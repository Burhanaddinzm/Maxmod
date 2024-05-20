using Maxmod.Enums;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Maxmod.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IVendorService _vendorService;

    public AccountService(UserManager<AppUser> userManager, IVendorService vendorService)
    {
        _userManager = userManager;
        _vendorService = vendorService;
    }

    public async Task<IdentityResult> Create(RegisterVM registerVM)
    {
        AppUser newUser = new AppUser
        {
            FullName = registerVM.FullName,
            UserName = registerVM.FullName.Trim().Replace(' ', '_') + "_" + Guid.NewGuid().ToString(),
            Email = registerVM.Email
        };

        var result = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (result.Succeeded)
        {
            if (registerVM.IsVendor)
            {
                var user = await _userManager.FindByEmailAsync(newUser.Email);
                await _vendorService.CreateVendorAsync(user!);
            }
            await _userManager.AddToRoleAsync(newUser, Roles.Customer.ToString());
        }

        return result;
    }

    public async Task<bool> CheckDuplicate(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<(AppUser?, IList<string>?)> CheckExistance(string email)
    {
        IList<string> roles = new List<string>();

        var user = await _userManager.FindByEmailAsync(email);
        if (user != null) roles = await _userManager.GetRolesAsync(user);

        return (user, roles);
    }
}
