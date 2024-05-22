using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.ViewComponents;

public class ProductSliderViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public ProductSliderViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products = await _productService.GetAllProductsAsync(x => x.IsNew, null, null, "ProductWeights", "Vendor", "ProductImages");
        return View(products);
    }
}
