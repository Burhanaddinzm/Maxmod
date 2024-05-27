using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Order : BaseAuditableEntity
{
    public int Quantity { get; set; }
    public int ProductWeightId { get; set; }
    public ProductWeight ProductWeight { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public bool IsComplete { get; set; }
}
