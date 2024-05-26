using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.ViewComponents;

public class CategorySliderViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public CategorySliderViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync(null, null, null, null, "SubCategories", "Products");
        return View(categories);
    }
}
