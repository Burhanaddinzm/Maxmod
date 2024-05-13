using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class NotificationController : Controller
{
    public IActionResult Index()
    {
        return View("Notification");
    }
}
