using Maxmod.Models.Common;

namespace Maxmod.Models;

public class ProductImage : BaseAuditableEntity
{
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
    public bool IsHover { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
