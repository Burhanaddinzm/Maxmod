namespace Maxmod.ViewModels.Cart;

public class CartVM
{
    public int Quantity { get; set; }
    public int ProductWeightId { get; set; }

    public static implicit operator CartVM(Models.Cart cart)
    {
        return new CartVM
        {
            Quantity = cart.Quantity,
            ProductWeightId = cart.ProductWeightId,
        };
    }
}
