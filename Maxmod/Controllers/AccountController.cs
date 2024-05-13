using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class AccountController : Controller
{
    readonly IAccountService _accountService;
    readonly SignInManager<AppUser> _signInManager;

    public AccountController(IAccountService accountService, SignInManager<AppUser> signInManager)
    {
        _accountService = accountService;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid) return View(loginVM);

        var (user, roles) = await _accountService.CheckExistance(loginVM.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid Credentials");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid Credentials");
            return View();
        }

        if (roles.Contains("Admin"))
        {
            return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
        }
        else return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid) return View(registerVM);

        if (await _accountService.CheckDuplicate(registerVM.Email))
        {
            ModelState.AddModelError("", "This user already exists!");
            return View(registerVM);
        }

        var result = await _accountService.Create(registerVM);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerVM);
        }

        return RedirectToAction("login");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("index", "home");
    }
}
