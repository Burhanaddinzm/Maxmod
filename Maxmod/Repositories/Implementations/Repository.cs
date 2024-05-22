using Maxmod.Data.Contexts;
using Maxmod.Models.Common;
using Maxmod.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        entity!.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync(
       Expression<Func<T, bool>>? where = null,
       Expression<Func<T, object>>? order = null,
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

        if (order != null)
        {
            query = query.OrderByDescending(order);
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

        if (expression == null) return await query.FirstOrDefaultAsync();

        if (includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
