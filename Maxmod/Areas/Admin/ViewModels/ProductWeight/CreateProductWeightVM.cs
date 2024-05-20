namespace Maxmod.Areas.Admin.ViewModels.ProductWeight;

public class CreateProductWeightVM
{
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int ProductId { get; set; }
    public int WeightId { get; set; }
}
