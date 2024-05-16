using Maxmod.Models;

namespace Maxmod.Areas.Admin.ViewModels.Vendor;

public class DeleteVendorVM
{
    public int Id { get; set; }
    public AppUser User { get; set; } = null!;
}
