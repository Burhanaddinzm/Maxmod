using Maxmod.Models;
using Maxmod.ViewModels.Category;
using System.Linq.Expressions;

namespace Maxmod.Services.Interfaces;

public interface ICategoryService
{
    Task CreateCategoryAsync(CreateCategoryVM createCategoryVM);
    Task UpdateCategoryAsync(UpdateCategoryVM updateCategoryVM, Category category);
    Task DeleteCategoryAsync(DeleteCategoryVM deleteCategoryVM);
    Task<List<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes);
    Task<Category> GetCategoryAsync(int id);
    Task<bool> CheckDuplicateAsync(string categoryName, int? categoryId = null);
    Task<(bool, Category)> CheckExistanceAsync(int id);
}
