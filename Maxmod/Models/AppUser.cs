using Microsoft.AspNetCore.Identity;

namespace Maxmod.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = null!;
    public override bool LockoutEnabled { get => true; set => base.LockoutEnabled = value; }
}
