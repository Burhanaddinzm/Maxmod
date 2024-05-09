using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
