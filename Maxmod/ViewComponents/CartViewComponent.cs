using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.ViewComponents;

public class CartViewComponent : ViewComponent
{
    private readonly ICartService _cartService;

    public CartViewComponent(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var cartItems = await _cartService.GetCartItemsAsync();
        return View(cartItems);
    }
}
