using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.ViewComponents;

public class HeaderCategoryViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public HeaderCategoryViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync(null, null, null, "Parent", "SubCategories");
        return View(categories);
    }
}
