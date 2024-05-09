using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
