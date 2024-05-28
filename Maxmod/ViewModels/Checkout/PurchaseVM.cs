namespace Maxmod.ViewModels.Checkout;

public class PurchaseVM
{
    public List<int> Quantity { get; set; }
    public List<int> ProductWeightId { get; set; }
    public string UserId { get; set; } = null!;
    public List<decimal> TotalPrice { get; set; }
    public string Nonce { get; set; } = null!;
    public PurchaseVM()
    {
        Quantity = new List<int>();
        ProductWeightId = new List<int>();
        TotalPrice = new List<decimal>();
    }
}
