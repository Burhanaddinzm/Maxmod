using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Maxmod.Services.Implementations;

public class LayoutService : ILayoutService
{
    readonly UserManager<AppUser> _userManager;
    readonly IHttpContextAccessor _contextAccessor;

    public LayoutService(UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task<string?> GetUserFullNameAsync()
    {
        var username = _contextAccessor.HttpContext?.User.Identity?.Name;
        if (username != null)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null) return user.FullName;
            else return null;
        }
        return null;
    }
}
