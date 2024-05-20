using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maxmod.Areas.Admin.ViewModels.Vendor;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class VendorController : Controller
{
    private readonly IVendorService _vendorService;

    public VendorController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    public async Task<IActionResult> Index()
    {
        var vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed, "User", "Products");
        return View(vendors);
    }

    public async Task<IActionResult> Requests()
    {
        var newVendors = await _vendorService.GetAllVendorsAsync(x => !x.IsConfirmed, "User");
        return View(newVendors);
    }

    public async Task<IActionResult> Update(int id)
    {
        var (exists, vendor) = await _vendorService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        var vendorVM = new UpdateVendorVM
        {
            Id = vendor!.Id,
            Name = vendor!.Name,
        };

        return View(vendorVM);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateVendorVM updateVendorVM)
    {
        if (!ModelState.IsValid) return View(updateVendorVM);

        var (exists, vendor) = await _vendorService.CheckExistanceAsync(updateVendorVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (await _vendorService.CheckDuplicateAsync(updateVendorVM.Name, updateVendorVM.Id))
        {
            ModelState.AddModelError("Name", "This vendor already exists!");
            return View(updateVendorVM);
        }

        var validationResult = await _vendorService.UpdateVendorAsync(updateVendorVM, vendor!);
        if (validationResult != null)
        {
            ModelState.AddModelError("Image", validationResult.ErrorMessage);
            return View(updateVendorVM);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id == 0)
        {
            TempData["Error"] = "Id is incorrect!";
            return RedirectToAction("Index", "Error", new { Area = "" });
        }

        var (exists, vendor) = await _vendorService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        return View(vendor);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(DeleteVendorVM deleteVendorVM)
    {
        var (exists, vendor) = await _vendorService.CheckExistanceAsync(deleteVendorVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        await _vendorService.DeleteVendorAsync(deleteVendorVM);
        return RedirectToAction("Index");
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
