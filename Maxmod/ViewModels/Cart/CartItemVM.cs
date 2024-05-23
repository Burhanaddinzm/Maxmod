namespace Maxmod.ViewModels.Cart;

public class CartItemVM
{
    private int quantity;
    private decimal price;
    private decimal discountPrice;

    public int ProductWeightId { get; set; }
    public string Product { get; set; } = null!;
    public int ProductId { get; set; }
    public string Weight { get; set; } = null!;
    public string ProductImage { get; set; } = null!;
    public int Quantity
    {
        get => quantity;
        set
        {
            quantity = value;
            UpdateTotalPrice();
        }
    }
    public decimal Price
    {
        get => price;
        set
        {
            price = value;
            UpdateTotalPrice();
        }
    }
    public decimal DiscountPrice
    {
        get => discountPrice;
        set
        {
            discountPrice = value;
            UpdateTotalPrice();
        }
    }
    public decimal TotalPrice { get; private set; }

    private void UpdateTotalPrice()
    {
        TotalPrice = (DiscountPrice != 0 ? DiscountPrice : Price) * Quantity;
    }
}
