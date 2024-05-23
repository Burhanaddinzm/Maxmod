using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Cart : BaseAuditableEntity
{
    public int Quantity { get; set; }
    public int ProductWeightId { get; set; }
    public ProductWeight ProductWeight { get; set; } = null!;
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
}
