namespace Maxmod.Areas.Admin.ViewModels.Vendor;

public class UpdateVendorVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public IFormFile? Image { get; set; }
}
