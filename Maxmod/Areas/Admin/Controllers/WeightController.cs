using Microsoft.AspNetCore.Mvc;
namespace Maxmod.Areas.Admin.Controllers;

public class WeightController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
