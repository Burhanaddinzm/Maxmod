﻿using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Category;
using Maxmod.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Maxmod.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IWebHostEnvironment _env;
    public CategoryService(ICategoryRepository categoryRepository, IWebHostEnvironment environment)
    {
        _categoryRepository = categoryRepository;
        _env = environment;
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

    public Task DeleteCategoryAsync(int id, DeleteCategoryVM deleteCategoryVM)
    {
        throw new NotImplementedException();
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await _categoryRepository.GetAsync(id);
    }

    public async Task<Category> GetCategoryAsync(string categoryName)
    {
        return await _categoryRepository.GetAsync(x => x.Name.Trim().ToLower() == categoryName.Trim().ToLower());
    }

    public Task UpdateCategoryAsync(int id, UpdateCategoryVM updateCategoryVM)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
    {
        return await _categoryRepository.GetAllAsync(expression, includes);
    }

    public async Task<bool> CheckDuplicateAsync(string categoryName)
    {
        var existingCategory = await GetCategoryAsync(categoryName);

        return existingCategory != null;
    }
}
