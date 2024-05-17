namespace Maxmod.Areas.Admin.ViewModels.Product;

public class CreateProductVM
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public IFormFile MainImage { get; set; } = null!;
    public IFormFile HoverImage { get; set; } = null!;
    public List<IFormFile>? AdditionalImages { get; set; } 
    public int CategoryId { get; set; }
    public int VendorId { get; set; }
}
