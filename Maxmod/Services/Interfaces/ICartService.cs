using Maxmod.ViewModels.Cart;

namespace Maxmod.Services.Interfaces;

public interface ICartService
{
    Task<List<CartItemVM>> GetCartItemsAsync();
    Task AddToCartAsync(int id, int quantity);
    Task RemoveCartItem(int id);
    void ClearCookies();
    Task<List<CartVM>> GetCart();
    Task MigrateToDBAsync();
}
