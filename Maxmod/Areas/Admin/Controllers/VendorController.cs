using Maxmod.Services.Implementations;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class VendorController : Controller
{
    readonly IVendorService _vendorService;

    public VendorController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Requests()
    {
        var newVendors = await _vendorService.GetAllVendorsAsync(x=>!x.IsConfirmed, "User");
        return View(newVendors);
    }

    public async Task<IActionResult> Accept(int id)
    {
        var (exists, vendor) = await _vendorService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        await _vendorService.AcceptVendor(vendor!);

        return RedirectToAction("Requests");
    }
    public async Task<IActionResult> Reject(int id)
    {
        var (exists, vendor) = await _vendorService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        await _vendorService.RejectVendor(vendor!);

        return RedirectToAction("Requests");
    }
}
