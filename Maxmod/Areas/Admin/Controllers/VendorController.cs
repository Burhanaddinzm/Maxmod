using Maxmod.Services.Implementations;
using Maxmod.Services.Interfaces;
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
        var newVendors = await _vendorService.GetAllVendorsAsync(null,"User");
        return View(newVendors);
    }
}
