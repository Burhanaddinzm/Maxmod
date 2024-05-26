using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.ViewComponents;

public class VendorViewComponent : ViewComponent
{
    private readonly IVendorService _vendorService;

    public VendorViewComponent(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var vendors = await _vendorService.GetAllVendorsAsync(x => x.IsConfirmed, null, null, 9);
        return View(vendors);
    }
}
