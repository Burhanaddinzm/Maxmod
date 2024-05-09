using Maxmod.Models.Common;

namespace Maxmod.Models;

public class ProductWeight : BaseEntity
{
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int WeightId { get; set; }
    public Weight Weight { get; set; } = null!;
}
