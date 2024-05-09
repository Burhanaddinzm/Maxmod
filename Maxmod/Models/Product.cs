using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<ProductImage>? ProductImages { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int VendorId { get; set; }
    public Vendor Vendor { get; set; } = null!;
    public bool IsNew { get; set; } = true;
    public List<ProductWeight> ProductWeights { get; set; }
    public Product()
    {
        ProductWeights = new List<ProductWeight>();
    }
}
