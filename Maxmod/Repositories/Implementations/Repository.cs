using Maxmod.Data.Contexts;
using Maxmod.Models.BaseModels;
using Maxmod.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Maxmod.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : BaseAuditable
{
    //readonly AppDbContext _context;
    //public Repository(AppDbContext context)
    //{
    //    _context = context;
    //}
    public async Task CreateAsync(T entity)
    {
        //await _context.Set<T>().AddAsync(entity);
        //await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression, params string[] includes)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
