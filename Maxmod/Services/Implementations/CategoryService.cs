using Maxmod.Extensions;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Category;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace Maxmod.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWebHostEnvironment _env;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CategoryService(
        ICategoryRepository categoryRepository,
        IWebHostEnvironment environment,
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _categoryRepository = categoryRepository;
        _env = environment;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateCategoryAsync(CreateCategoryVM createCategoryVM)
    {
        var filename = await createCategoryVM.Image.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "category");
        Category category = new Category
        {
            Name = createCategoryVM.Name,
            ParentId = createCategoryVM.ParentId,
        };
        category.Image = filename;

        await _categoryRepository.CreateAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id, DeleteCategoryVM deleteCategoryVM)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

        if (id != deleteCategoryVM.Id)
        {
            tempData["Error"] = "Id is incorrect";
            return false;
        }
        await _categoryRepository.DeleteAsync(deleteCategoryVM.Id);
        return true;
    }

    public Task UpdateCategoryAsync(int id, UpdateCategoryVM updateCategoryVM)
    {
        throw new NotImplementedException();
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await _categoryRepository.GetAsync(id);
    }

    public async Task<List<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
    {
        return await _categoryRepository.GetAllAsync(expression, includes);
    }

    public async Task<bool> CheckDuplicateAsync(string categoryName)
    {
        var existingCategory = await _categoryRepository.GetAsync(x => x.Name.Trim().ToLower() == categoryName.Trim().ToLower());

        return existingCategory != null;
    }
}
