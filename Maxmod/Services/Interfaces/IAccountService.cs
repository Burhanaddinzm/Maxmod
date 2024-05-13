using Maxmod.Models;
using Maxmod.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace Maxmod.Services.Interfaces;

public interface IAccountService
{
    Task<IdentityResult> Create(RegisterVM registerVM);
    Task<bool> CheckDuplicate(string email);
    Task<(AppUser?, IList<string>?)> CheckExistance(string email);
}
