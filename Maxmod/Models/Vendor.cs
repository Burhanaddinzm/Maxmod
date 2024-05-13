using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Vendor : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public bool IsConfirmed { get; set; }
    public List<Product>? Products { get; set; }
}
