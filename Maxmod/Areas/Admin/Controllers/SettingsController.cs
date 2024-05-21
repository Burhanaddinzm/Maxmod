using Maxmod.Areas.Admin.ViewModels.Category;
using Maxmod.Areas.Admin.ViewModels.Settings;
using Maxmod.Models;
using Maxmod.Services.Implementations;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class SettingsController : Controller
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task<IActionResult> Index()
    {
        var settings = await _settingsService.GetSettingsAsync();
        return View(settings);
    }

    public async Task<IActionResult> Update(int id)
    {
        var settings = await _settingsService.GetSettingsAsync();
        UpdateSettingsVM vm = settings;
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateSettingsVM updateSettingsVM)
    {
        if (!ModelState.IsValid) return View(updateSettingsVM);

        var validationResult = await _settingsService.UpdateSettingsAsync(updateSettingsVM);
        if (validationResult != null)
        {
            ModelState.AddModelError("", validationResult.ErrorMessage);
            return View(updateSettingsVM);
        }
        return RedirectToAction("Index");
    }
}
