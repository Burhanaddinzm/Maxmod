using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Maxmod.ViewModels.Account;

public class RegisterVM
{
    [DisplayName("Full Name")]
    public string FullName { get; set; } = null!;
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [DisplayName("Confirm Password")]
    [DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
    public bool IsVendor { get; set; }
}
