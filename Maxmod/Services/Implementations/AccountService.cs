using Maxmod.Enums;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Maxmod.Services.Implementations;

public class AccountService : IAccountService
{
    readonly UserManager<AppUser> _userManager;

    public AccountService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> Create(RegisterVM registerVM)
    {
        AppUser newUser = new AppUser
        {
            FullName = registerVM.FullName,
            UserName = registerVM.FullName.Trim().Replace(' ', '_'),
            Email = registerVM.Email
        };

        var result = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (result.Succeeded) await _userManager.AddToRoleAsync(newUser, Roles.Customer.ToString());

        return result;
    }

    public async Task<bool> CheckDuplicate(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<(AppUser?,IList<string>?)> CheckExistance(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var roles = await _userManager.GetRolesAsync(user);
        return (user,roles);
    }
}
