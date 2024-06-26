﻿using Maxmod.Areas.Admin.ViewModels.Category;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public async Task<IActionResult> Index()
    {
        List<Category>? categories = await _categoryService.GetAllCategoriesAsync(null, null, null, null, "Products");
        return View(categories);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId == null);
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId == null);

        if (!ModelState.IsValid) return View(categoryVM);
        if (await _categoryService.CheckDuplicateAsync(categoryVM.Name))
        {
            ModelState.AddModelError("Name", "This category already exists!");
            return View(categoryVM);
        }

        var validationResult = await _categoryService.CreateCategoryAsync(categoryVM);
        if (validationResult != null)
        {
            ModelState.AddModelError("Image", validationResult.ErrorMessage);
            return View(categoryVM);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId == null && x.Id != id);

        var (exists, category) = await _categoryService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        UpdateCategoryVM vm = category!;

        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryVM updateCategoryVM)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId == null && x.Id != updateCategoryVM.Id);

        if (!ModelState.IsValid) return View(updateCategoryVM);

        var (exists, category) = await _categoryService.CheckExistanceAsync(updateCategoryVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (await _categoryService.CheckDuplicateAsync(updateCategoryVM.Name, updateCategoryVM.Id))
        {
            ModelState.AddModelError("Name", "This category already exists!");
            return View(updateCategoryVM);
        }

        var validationResult = await _categoryService.UpdateCategoryAsync(updateCategoryVM, category!);
        if (validationResult != null)
        {
            ModelState.AddModelError("Image", validationResult.ErrorMessage);
            return View(updateCategoryVM);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId == null && x.Id != id);

        if (id == 0)
        {
            TempData["Error"] = "Id is incorrect!";
            return RedirectToAction("Index", "Error", new { Area = "" });
        }

        var (exists, category) = await _categoryService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        return View(category);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(DeleteCategoryVM deleteCategoryVM)
    {
        var (exists, category) = await _categoryService.CheckExistanceAsync(deleteCategoryVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        await _categoryService.DeleteCategoryAsync(deleteCategoryVM);
        return RedirectToAction("Index");
    }
}
