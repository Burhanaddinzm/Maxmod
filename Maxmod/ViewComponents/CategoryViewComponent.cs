using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.ViewComponents;

public class CategoryViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public CategoryViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync(null, "Parent", "SubCategories");
        return View(categories);
    }
}
