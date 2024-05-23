using Azure;
using Azure.Core;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;

namespace Maxmod.Services.Implementations;

public class CartService : ICartService
{
    private readonly IProductWeightService _productWeightService;
    private readonly ILayoutService _layoutService;
    private readonly IHttpContextAccessor _accessor;

    public CartService(
        IProductWeightService productWeightService,
        ILayoutService layoutService,
        IHttpContextAccessor accessor)
    {
        _productWeightService = productWeightService;
        _layoutService = layoutService;
        _accessor = accessor;
    }

    public async Task<List<CartItemVM>> GetCartItemsAsync()
    {
        List<CartVM> cartVM = GetCart();
        List<CartItemVM>? cartItems = new List<CartItemVM>();

        foreach (var item in cartVM)
        {
            var (exist, productWeight) = await _productWeightService.CheckExistanceAsync(item.ProductWeightId);

            CartItemVM cartItem = new CartItemVM
            {
                ProductWeightId = productWeight!.Id,
                Product = productWeight.Product.Name,
                ProductId = productWeight.ProductId,
                Weight = productWeight.Weight.Name,
                ProductImage = productWeight.Product.ProductImages!.FirstOrDefault(x=> x.IsMain)!.Url,
                Quantity = item.Quantity,
                Price = productWeight.Price,
                DiscountPrice = productWeight.DiscountPrice,
            };

            cartItems.Add(cartItem);
        }
        return cartItems;
    }

    public async Task<bool> AddToCartAsync(int id, int quantity)
    {
        if (!_layoutService.CheckLoggedIn())
        {
            var (exists, existingProductWeight) = await _productWeightService.CheckExistanceAsync(id);
            if (!exists)
            {
                return exists;
            }

            List<CartVM> Cart = GetCart();
            CartVM cartVm = Cart.Find(x => x.ProductWeightId == existingProductWeight!.Id)!;

            if (cartVm != null)
            {
                cartVm.Quantity += quantity;
            }
            else
            {
                Cart.Add(new CartVM { Quantity = quantity, ProductWeightId = existingProductWeight!.Id });
            }

            _accessor.HttpContext!.Response.Cookies.Append("Cart", JsonConvert.SerializeObject(Cart), new CookieOptions { Expires = DateTime.MaxValue });
        }
        return true;
    }

    public void ClearCookies()
    {
        if (_accessor.HttpContext!.Request.Cookies["Cart"] != null)
        {
            _accessor.HttpContext!.Response.Cookies.Delete("Cart");
        }
    }

    public void RemoveCartItem(int id)
    {
        if (!_layoutService.CheckLoggedIn())
        {
            List<CartVM> Cart = GetCart();
            if (Cart != null && Cart.Count > 0)
            {
                var itemToRemove = Cart.FirstOrDefault(x => x.ProductWeightId == id);

                if (itemToRemove != null) Cart.Remove(itemToRemove);

                _accessor.HttpContext!.Response.Cookies.Append("Cart", JsonConvert.SerializeObject(Cart), new CookieOptions { Expires = DateTime.MaxValue });
            }
        }
    }

    public List<CartVM> GetCart()
    {
        if (!_layoutService.CheckLoggedIn())
        {
            List<CartVM> CartVMs;
            if (_accessor.HttpContext!.Request.Cookies["Cart"] != null)
            {
                CartVMs = JsonConvert.DeserializeObject<List<CartVM>>(_accessor.HttpContext!.Request.Cookies["Cart"]!)!;
            }
            else CartVMs = new List<CartVM>();

            return CartVMs;
        }
        return new List<CartVM>();
    }
}
