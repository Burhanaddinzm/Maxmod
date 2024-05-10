using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Category;

namespace Maxmod.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IHostEnvironment _env;
    public CategoryService(ICategoryRepository categoryRepository, IHostEnvironment environment)
    {
        _categoryRepository = categoryRepository;
        _env = environment;
    }

    public Task<CreateCategoryVM> CreateCategoryAsync(CreateCategoryVM createCategoryVM)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategoryAsync(int id, DeleteCategoryVM deleteCategoryVM)
    {
        throw new NotImplementedException();
    }

    public Task<List<CategoryVM>> GetAllCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<CategoryVM> GetCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateCategoryVM> UpdateCategoryAsync(int id, UpdateCategoryVM updateCategoryVM)
    {
        throw new NotImplementedException();
    }
}
