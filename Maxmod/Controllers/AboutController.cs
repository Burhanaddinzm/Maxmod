using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
