using Maxmod.Data.Contexts;
using Maxmod.Models.Common;
using Maxmod.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Maxmod.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : BaseAuditableEntity
{
    readonly AppDbContext _context;
    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? where = null,
        string? order = null,
        string? orderByDesc = null,
        int? take = null,
        params string[] includes)
    {
        IQueryable<T> query = _context.Set<T>().AsQueryable();

        if (includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (where != null)
        {
            query = query.Where(where);
        }

        if (!string.IsNullOrEmpty(order))
        {
            query = ApplyOrderBy(query, order, false);
        }

        if (!string.IsNullOrEmpty(orderByDesc))
        {
            query = ApplyOrderBy(query, orderByDesc, true);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>>? expression, params string[] includes)
    {
        IQueryable<T> query = _context.Set<T>().AsQueryable();

        if (includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (expression != null)
        {
            return await query.FirstOrDefaultAsync(expression);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    private IQueryable<T> ApplyOrderBy(IQueryable<T> query, string propertyName, bool descending)
    {
        var propertyInfo = typeof(T).GetProperty(propertyName);
        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, propertyInfo);
        var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpression, typeof(object)), parameter);

        return descending ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
    }
}
