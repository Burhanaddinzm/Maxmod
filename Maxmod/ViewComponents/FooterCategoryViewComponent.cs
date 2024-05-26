using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.ViewComponents;

public class FooterCategoryViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public FooterCategoryViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId == null, null, null, 4);
        return View(categories);
    }
}
