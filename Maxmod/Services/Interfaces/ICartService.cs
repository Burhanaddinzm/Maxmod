using Maxmod.ViewModels.Cart;

namespace Maxmod.Services.Interfaces;

public interface ICartService
{
    Task<List<CartItemVM>> GetCartItemsAsync();
    Task<bool> AddToCartAsync(int id, int quantity);
    void RemoveCartItem(int id);
    void ClearCookies();
    List<CartVM> GetCart();
}
