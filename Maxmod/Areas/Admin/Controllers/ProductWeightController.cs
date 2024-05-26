using Maxmod.Areas.Admin.ViewModels.ProductWeight;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class ProductWeightController : Controller
{
    private readonly IProductWeightService _productWeightService;
    private readonly IProductService _productService;
    private readonly IWeightService _weightService;

    public ProductWeightController(
        IProductWeightService productWeightService,
        IProductService productService,
        IWeightService weightService)
    {
        _productWeightService = productWeightService;
        _productService = productService;
        _weightService = weightService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductWeight>? productWeights;
        if (User.IsInRole("Vendor"))
        {
            productWeights = await _productWeightService.GetAllProductWeightsAsync(
                x => x.Product.Vendor.User.UserName == User.Identity!.Name, null, null, null,
                "Product", "Weight");
        }
        else productWeights = await _productWeightService.GetAllProductWeightsAsync(null, null, null, null, "Product", "Weight");
        return View(productWeights);
    }

    public async Task<IActionResult> Create()
    {
        if (User.IsInRole("Vendor"))
            ViewBag.Products = await _productService.GetAllProductsAsync(x => x.Vendor.User.UserName == User.Identity!.Name);
        else
            ViewBag.Products = await _productService.GetAllProductsAsync();
        ViewBag.Weights = await _weightService.GetAllWeightsAsync(x=> x.Name != "Default");

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductWeightVM createProductWeightVM)
    {
        if (User.IsInRole("Vendor"))
            ViewBag.Products = await _productService.GetAllProductsAsync(x => x.Vendor.User.UserName == User.Identity!.Name);
        else
            ViewBag.Products = await _productService.GetAllProductsAsync();
        ViewBag.Weights = await _weightService.GetAllWeightsAsync(x => x.Name != "Default");

        if (!ModelState.IsValid) return View(createProductWeightVM);

        if (await _productWeightService.CheckDuplicateAsync(createProductWeightVM))
        {
            ModelState.AddModelError("", "This Product-Weight already exists!");
            return View(createProductWeightVM);
        }

        await _productWeightService.CreateProductWeightAsync(createProductWeightVM);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        if (User.IsInRole("Vendor"))
            ViewBag.Products = await _productService.GetAllProductsAsync(x => x.Vendor.User.UserName == User.Identity!.Name);
        else
            ViewBag.Products = await _productService.GetAllProductsAsync();
        ViewBag.Weights = await _weightService.GetAllWeightsAsync();

        var (exists, productWeight) = await _productWeightService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (User.IsInRole("Vendor"))
        {
            if (productWeight!.Product.Vendor.User.UserName != User.Identity!.Name)
                return RedirectToAction("Index", "Error", new { Area = "" });
        }

        var updateProductWeightVM = new UpdateProductWeightVM
        {
            Id = productWeight!.Id,
            Stock = productWeight!.Stock,
            Price = productWeight!.Price,
            DiscountPrice = productWeight!.DiscountPrice,
            ProductId = productWeight!.ProductId,
            WeightId = productWeight!.WeightId,
        };
        return View(updateProductWeightVM);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductWeightVM updateProductWeightVM)
    {
        if (User.IsInRole("Vendor"))
            ViewBag.Products = await _productService.GetAllProductsAsync(x => x.Vendor.User.UserName == User.Identity!.Name);
        else
            ViewBag.Products = await _productService.GetAllProductsAsync();
        ViewBag.Weights = await _weightService.GetAllWeightsAsync();

        if (!ModelState.IsValid) return View(updateProductWeightVM);

        var (exists, productWeight) = await _productWeightService.CheckExistanceAsync(updateProductWeightVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (await _productWeightService.CheckDuplicateAsync(null, updateProductWeightVM))
        {
            ModelState.AddModelError("", "This Product-Weight already exists!");
            return View(updateProductWeightVM);
        }

        await _productWeightService.UpdateProductWeightAsync(updateProductWeightVM, productWeight!);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (User.IsInRole("Vendor"))
            ViewBag.Products = await _productService.GetAllProductsAsync(x => x.Vendor.User.UserName == User.Identity!.Name);
        else
            ViewBag.Products = await _productService.GetAllProductsAsync();
        ViewBag.Weights = await _weightService.GetAllWeightsAsync();

        if (id == 0)
        {
            TempData["Error"] = "Id is incorrect!";
            return RedirectToAction("Index", "Error", new { Area = "" });
        }

        var (exists, productWeight) = await _productWeightService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (User.IsInRole("Vendor"))
        {
            if (productWeight!.Product.Vendor.User.UserName != User.Identity!.Name)
                return RedirectToAction("Index", "Error", new { Area = "" });
        }

        return View(productWeight);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(DeleteProductWeightVM deleteProductWeightVM)
    {
        if (User.IsInRole("Vendor"))
            ViewBag.Products = await _productService.GetAllProductsAsync(x => x.Vendor.User.UserName == User.Identity!.Name);
        else
            ViewBag.Products = await _productService.GetAllProductsAsync();
        ViewBag.Weights = await _weightService.GetAllWeightsAsync();

        var (exists, productWeight) = await _productWeightService.CheckExistanceAsync(deleteProductWeightVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        await _productWeightService.DeleteProductWeightAsync(deleteProductWeightVM);
        return RedirectToAction("Index");
    }
}
