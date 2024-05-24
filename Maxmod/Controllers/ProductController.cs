using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var products = await _productService.GetAllProductsAsync(null, null, null, "ProductWeights.Weight", "Vendor", "ProductImages");

        const int pageSize = 6;
        if (page < 1) page = 1;

        int itemCount = products.Count();

        var pager = new PagerVM(itemCount, page, pageSize);

        int itemsToSkip = (page - 1) * pageSize;

        var data = products.Skip(itemsToSkip).Take(pager.PageSize).ToList();

        ViewBag.Pager = pager;

        return View(data);
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
