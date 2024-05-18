using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class ProductImageController : Controller
{
    readonly IProductImageService _productImageService;

    public ProductImageController(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }
    public async Task<IActionResult> GetImages(int id)
    {
        var productImages = await _productImageService.GetProductImagesAsync(id);

        if (productImages == null) return RedirectToAction("Index", "Error", new { Area = "" });
        return PartialView("_ProductImagePartial", productImages);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var exist = await _productImageService.DeleteProductImageAsync(id);

        if (!exist) return RedirectToAction("Index", "Error", new { Area = "" });
        return Json("");
    }
}
