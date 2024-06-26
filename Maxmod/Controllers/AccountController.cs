﻿using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ICartService _cartService;

    public AccountController(IAccountService accountService, SignInManager<AppUser> signInManager, ICartService cartService)
    {
        _accountService = accountService;
        _signInManager = signInManager;
        _cartService = cartService;
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
            return View(loginVM);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

        if (!result.Succeeded)
        {
            if (!result.IsLockedOut) ModelState.AddModelError("", "Invalid Credentials");
            if (result.IsLockedOut) ModelState.AddModelError("", "You have been locked out, try again in 5 minutes!");
            return View(loginVM);
        }

        await _cartService.MigrateToDBAsync();

        if (roles!.Contains("Admin"))
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

        if (registerVM.IsVendor)
        {
            TempData["Notification"] = "Your vendor request is sent to the Maxmod team. You will be notified by email.";
            return View("Notification");
        }
        else return RedirectToAction("login");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("index", "home");
    }
}
