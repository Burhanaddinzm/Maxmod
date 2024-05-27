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
        var products = await _productService.GetAllProductsAsync(x => x.IsNew && x.ProductWeights.Any(x => x.Stock > 0), null, "CreatedAt", 10, "ProductWeights.Weight", "Vendor", "ProductImages");
        return View(products);
    }
}
