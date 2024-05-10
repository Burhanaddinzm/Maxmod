using Maxmod.Models;
using Maxmod.ViewModels.Category;
using System.Linq.Expressions;

namespace Maxmod.Services.Interfaces;

public interface ICategoryService
{
    Task CreateCategoryAsync(CreateCategoryVM createCategoryVM);
    Task UpdateCategoryAsync(int id, UpdateCategoryVM updateCategoryVM);
    Task DeleteCategoryAsync(int id, DeleteCategoryVM deleteCategoryVM);
    Task<List<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes);
    Task<Category> GetCategoryAsync(int id);
    Task<Category> GetCategoryAsync(string categoryName);
    Task<bool> CheckDuplicateAsync(string categoryName);
}
