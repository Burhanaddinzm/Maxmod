using Maxmod.Areas.Admin.ViewModels.Product;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class ProductController : Controller
{
    readonly IProductService _productService;
    readonly ICategoryService _categoryService;
    readonly IVendorService _vendorService;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService,
        IVendorService vendorService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _vendorService = vendorService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync(null, "ProductImages", "Category", "Vendor");
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Vendors = await _vendorService.GetAllVendorsAsync();

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductVM createProductVM)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Vendors = await _vendorService.GetAllVendorsAsync();

        if (!ModelState.IsValid) return View(createProductVM);

        if (await _productService.CheckDuplicateAsync(createProductVM.Name))
        {
            ModelState.AddModelError("Name", "This product already exists!");
            return View(createProductVM);
        }

        var validationResult = await _productService.CreateProductAsync(createProductVM);

        if (validationResult != null)
        {
            ModelState.AddModelError("", validationResult.ErrorMessage);
            return View(createProductVM);
        }
        return RedirectToAction("Index");
    }
}
