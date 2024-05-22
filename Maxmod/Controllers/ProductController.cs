using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Detail(int id)
    {
        var (exists, product) = await _productService.CheckExistanceAsync(id);
        if (!exists) RedirectToAction("Index", "Error");
        if (!product!.ProductWeights.Any(x => x.Stock > 0))
        {
            TempData["Error"] = "Product not found!";
            return RedirectToAction("Index", "Error");
        }
        return View(product);
    }
}
