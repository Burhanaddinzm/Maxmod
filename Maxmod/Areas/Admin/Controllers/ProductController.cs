using Maxmod.Areas.Admin.ViewModels.Product;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IVendorService _vendorService;

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
        List<Product>? products;
        if (User.IsInRole("Vendor"))
        {
            products = await _productService.GetAllProductsAsync(x => x.Vendor.User.UserName == User.Identity!.Name, null, null,
                "ProductImages", "Category", "Vendor");
        }
        else products = await _productService.GetAllProductsAsync(null, null, null, "ProductImages", "Category", "Vendor");
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId != null);
        if (User.IsInRole("Vendor"))
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed && x.User.UserName == User.Identity!.Name);
        else
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed);

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductVM createProductVM)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId != null);
        if (User.IsInRole("Vendor"))
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed && x.User.UserName == User.Identity!.Name);
        else
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed);

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

    public async Task<IActionResult> Update(int id)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId != null);
        if (User.IsInRole("Vendor"))
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed && x.User.UserName == User.Identity!.Name);
        else
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed);

        var (exists, product) = await _productService.CheckExistanceAsync(id);
        if (!exists)
        {
            return RedirectToAction("Index", "Error", new { Area = "" });
        }

        if (User.IsInRole("Vendor"))
        {
            if (product!.Vendor.User.UserName != User.Identity!.Name)
                return RedirectToAction("Index", "Error", new { Area = "" });
        }

        var productVm = new UpdateProductVM
        {
            Id = product!.Id,
            Name = product!.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            VendorId = product.VendorId,
        };
        return View(productVm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductVM updateProductVM)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId != null);
        if (User.IsInRole("Vendor"))
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed && x.User.UserName == User.Identity!.Name);
        else
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed);

        if (!ModelState.IsValid) return View(updateProductVM);

        var (exists, product) = await _productService.CheckExistanceAsync(updateProductVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (await _productService.CheckDuplicateAsync(updateProductVM.Name, updateProductVM.Id))
        {
            ModelState.AddModelError("Name", "This product already exists!");
            return View(updateProductVM);
        }

        var validationResult = await _productService.UpdateProductAsync(updateProductVM, product!);

        if (validationResult != null)
        {
            ModelState.AddModelError("", validationResult.ErrorMessage);
            return View(updateProductVM);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId != null);
        if (User.IsInRole("Vendor"))
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed && x.User.UserName == User.Identity!.Name);
        else
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed);

        if (id == 0)
        {
            TempData["Error"] = "Id is incorrect!";
            return RedirectToAction("Index", "Error", new { Area = "" });
        }

        var (exists, product) = await _productService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (User.IsInRole("Vendor"))
        {
            if (product!.Vendor.User.UserName != User.Identity!.Name)
                return RedirectToAction("Index", "Error", new { Area = "" });
        }

        return View(product);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(DeleteProductVM deleteProductVM)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync(x => x.ParentId != null);
        if (User.IsInRole("Vendor"))
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed && x.User.UserName == User.Identity!.Name);
        else
            ViewBag.Vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed);

        var (exists, product) = await _categoryService.CheckExistanceAsync(deleteProductVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        await _productService.DeleteProductAsync(deleteProductVM);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Detail(int id)
    {
        var (exists, product) = await _productService.CheckExistanceAsync(id);
        if (!exists)
        {
            return RedirectToAction("Index", "Error", new { Area = "" });
        }

        if (User.IsInRole("Vendor"))
        {
            if (product!.Vendor.User.UserName != User.Identity!.Name)
                return RedirectToAction("Index", "Error", new { Area = "" });
        }

        return View(product);
    }
}
