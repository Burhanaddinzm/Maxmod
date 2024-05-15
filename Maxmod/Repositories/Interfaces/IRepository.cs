using Maxmod.Models.Common;
using System.Linq.Expressions;

namespace Maxmod.Repositories.Interfaces;

public interface IRepository<T> where T : BaseAuditableEntity
{
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, params string[] includes);
    Task<T?> GetAsync(int id);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression, params string[] includes);
}
