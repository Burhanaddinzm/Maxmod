using Maxmod.Areas.Admin.ViewModels.Category;
using Maxmod.Areas.Admin.ViewModels.Weight;
using Maxmod.Models;
using Maxmod.Services.Implementations;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class WeightController : Controller
{
    private readonly IWeightService _weightService;

    public WeightController(IWeightService weightService)
    {
        _weightService = weightService;
    }

    public async Task<IActionResult> Index()
    {
        List<Weight>? weights = await _weightService.GetAllWeightsAsync();
        return View(weights);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateWeightVM createWeightVM)
    {
        if (!ModelState.IsValid) return View(createWeightVM);

        if (await _weightService.CheckDuplicateAsync(createWeightVM.Name))
        {
            ModelState.AddModelError("Name", "This weight already exists!");
            return View(createWeightVM);
        }

        await _weightService.CreateWeightAsync(createWeightVM);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var (exists, weight) = await _weightService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        return View(new UpdateWeightVM { Id = weight!.Id, Name = weight!.Name });
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateWeightVM updateWeightVM)
    {
        if (!ModelState.IsValid) return View(updateWeightVM);

        var (exists, weight) = await _weightService.CheckExistanceAsync(updateWeightVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        if (await _weightService.CheckDuplicateAsync(updateWeightVM.Name, updateWeightVM.Id))
        {
            ModelState.AddModelError("Name", "This weight already exists!");
            return View(updateWeightVM);
        }

        await _weightService.UpdateWeightAsync(updateWeightVM, weight!);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id == 0)
        {
            TempData["Error"] = "Id is incorrect!";
            return RedirectToAction("Index", "Error", new { Area = "" });
        }

        var (exists, weight) = await _weightService.CheckExistanceAsync(id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        return View(weight);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(DeleteWeightVM deleteWeightVM)
    {
        var (exists, weight) = await _weightService.CheckExistanceAsync(deleteWeightVM.Id);

        if (!exists) return RedirectToAction("Index", "Error", new { Area = "" });

        await _weightService.DeleteWeightAsync(deleteWeightVM);
        return RedirectToAction("Index");
    }
}
