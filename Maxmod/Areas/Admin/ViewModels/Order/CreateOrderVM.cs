
namespace Maxmod.Areas.Admin.ViewModels.Order;

public class CreateOrderVM
{
    public int Quantity { get; set; }
    public int ProductWeightId { get; set; }
    public string UserId { get; set; } = null!;
    public decimal TotalPrice { get; set; }
}
