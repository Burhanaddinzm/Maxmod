using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers
{
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
            List<Category>? categories = await _categoryService.GetAllCategoriesAsync(null, "Products");
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(null, "Products");
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
    }
}
