using Maxmod.Areas.Admin.ViewModels.Category;
using Maxmod.Models;
using System.Linq.Expressions;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Interfaces;

public interface ICategoryService
{
    Task<FileValidationResult?> CreateCategoryAsync(CreateCategoryVM createCategoryVM);
    Task<FileValidationResult?> UpdateCategoryAsync(UpdateCategoryVM updateCategoryVM, Category category);
    Task DeleteCategoryAsync(DeleteCategoryVM deleteCategoryVM);
    Task<List<Category>> GetAllCategoriesAsync(
       Expression<Func<Category, bool>>? where = null,
       Expression<Func<Category, object>>? order = null,
       int? take = null,
       params string[] includes);
    Task<Category> GetCategoryAsync(int id);
    Task<bool> CheckDuplicateAsync(string categoryName, int? categoryId = null);
    Task<(bool, Category?)> CheckExistanceAsync(int id);
}
