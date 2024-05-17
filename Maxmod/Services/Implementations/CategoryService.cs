using Maxmod.Areas.Admin.ViewModels.Category;
using Maxmod.Areas.Admin.ViewModels.Vendor;
using Maxmod.Extensions;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
using static Maxmod.Extensions.FileExtension;

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

    public async Task<FileValidationResult?> CreateCategoryAsync(CreateCategoryVM createCategoryVM)
    {
        var validationResult = createCategoryVM.Image.ValidateFile();
        if (!validationResult.IsValid) return validationResult;

        var filename = await createCategoryVM.Image.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "category");
        Category category = new Category
        {
            Name = createCategoryVM.Name,
            ParentId = createCategoryVM.ParentId,
        };
        category.Image = filename;

        await _categoryRepository.CreateAsync(category);
        return null;
    }

    public async Task DeleteCategoryAsync(DeleteCategoryVM deleteCategoryVM)
    {
        await _categoryRepository.DeleteAsync(deleteCategoryVM.Id);
    }

    public async Task<FileValidationResult?> UpdateCategoryAsync(UpdateCategoryVM updateCategoryVM, Category category)
    {
        if (updateCategoryVM.Image != null)
        {
            var validationResult = updateCategoryVM.Image.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateCategoryVM.Image.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "category");
            updateCategoryVM.Image.DeleteFile(_env.WebRootPath, "client", "assets", "images", "category", category.Image);
            category.Image = filename;
        }

        category.Name = updateCategoryVM.Name;
        category.ParentId = updateCategoryVM.ParentId;

        await _categoryRepository.UpdateAsync(category);
        return null;
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await _categoryRepository.GetAsync(id);
    }

    public async Task<List<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
    {
        return await _categoryRepository.GetAllAsync(expression, includes);
    }

    public async Task<bool> CheckDuplicateAsync(string categoryName, int? categoryId = null)
    {
        Category? existingCategory;

        if (categoryId != null)
        {
            existingCategory = await _categoryRepository.GetAsync(
                x => x.Name.Trim().ToLower() == categoryName.Trim().ToLower() &&
                x.Id != categoryId
                );
        }
        else existingCategory = await _categoryRepository.GetAsync(x => x.Name.Trim().ToLower() == categoryName.Trim().ToLower());

        return existingCategory != null;
    }

    public async Task<(bool, Category?)> CheckExistanceAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

        Category? category = await _categoryRepository.GetAsync(id);

        if (category == null)
            tempData["Error"] = "Category not found!";

        return (category != null, category);
    }
}
