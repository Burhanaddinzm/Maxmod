using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View("Error");
    }
}
