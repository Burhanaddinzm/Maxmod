using Maxmod.ViewModels.Category;

namespace Maxmod.Services.Interfaces;

public interface ICategoryService
{
    Task<CreateCategoryVM> CreateCategoryAsync(CreateCategoryVM createCategoryVM);
    Task<UpdateCategoryVM> UpdateCategoryAsync(int id, UpdateCategoryVM updateCategoryVM);
    Task DeleteCategoryAsync(int id, DeleteCategoryVM deleteCategoryVM);
    Task<List<CategoryVM>> GetAllCategoriesAsync();
    Task<CategoryVM> GetCategoryAsync(int id);
}
