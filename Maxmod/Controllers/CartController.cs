using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var cartItems = await _cartService.GetCartItemsAsync();
        return View(cartItems);
    }

    public async Task<IActionResult> AddToCart(int id, int quantity)
    {
        await _cartService.AddToCartAsync(id, quantity);
        return Json("");
    }

    public IActionResult RemoveFromCart(int id)
    {
        _cartService.RemoveCartItem(id);
        return Json("");
    }
}
