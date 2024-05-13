using Maxmod.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(LoginVM loginVM)
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(RegisterVM registerVM)
    {
        return View();
    }
}
