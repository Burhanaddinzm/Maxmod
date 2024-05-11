using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxmod.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            List<Category>? categories = await _categoryService.GetAllCategoriesAsync(null, "Products");
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            if (!ModelState.IsValid) return View(categoryVM);
            if (await _categoryService.CheckDuplicateAsync(categoryVM.Name))
            {
                ModelState.AddModelError("Name", "This category already exists!");
                return View(categoryVM);
            }
            await _categoryService.CreateCategoryAsync(categoryVM);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

            if (id == 0)
            {
                TempData["Error"] = "Id is incorrect!";
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

            Category? category = await _categoryService.GetCategoryAsync(id);

            if (category == null)
            {
                TempData["Error"] = "Category not found!";
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id, DeleteCategoryVM deleteCategoryVM)
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

            var result = await _categoryService.DeleteCategoryAsync(id, deleteCategoryVM);
            if (!result)
            {
                TempData["Error"] = "Failed to delete category!";
                return RedirectToAction("Error", "Home", new { Area = "" });
            }

            return RedirectToAction("Index");
        }
    }
}
