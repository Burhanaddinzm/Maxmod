using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class ProductController : Controller
{
    readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync(null, "ProductImages", "Category", "Vendor");
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }
    //[HttpPost]
    //public async Task<IActionResult> Create()
    //{
    //    return RedirectToAction("Index");
    //}
}
