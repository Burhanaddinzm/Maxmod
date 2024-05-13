using System.ComponentModel.DataAnnotations;

namespace Maxmod.ViewModels.Account;

public class LoginVM
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}
