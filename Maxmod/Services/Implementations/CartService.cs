using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Cart;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Maxmod.Services.Implementations;

public class CartService : ICartService
{
    private readonly IProductWeightService _productWeightService;
    private readonly ICartRepository _cartRepository;
    private readonly ILayoutService _layoutService;
    private readonly IHttpContextAccessor _accessor;
    private readonly UserManager<AppUser> _userManager;

    public CartService(
        IProductWeightService productWeightService,
        ILayoutService layoutService,
        IHttpContextAccessor accessor,
        ICartRepository cartRepository,
        UserManager<AppUser> userManager)
    {
        _productWeightService = productWeightService;
        _layoutService = layoutService;
        _accessor = accessor;
        _cartRepository = cartRepository;
        _userManager = userManager;
    }

    public async Task<List<CartItemVM>> GetCartItemsAsync()
    {
        List<CartVM> cartVM = await GetCart();
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
                ProductImage = productWeight.Product.ProductImages!.FirstOrDefault(x => x.IsMain)!.Url,
                Quantity = item.Quantity,
                Price = productWeight.Price,
                DiscountPrice = productWeight.DiscountPrice,
            };

            cartItems.Add(cartItem);
        }
        return cartItems;
    }

    public async Task AddToCartAsync(int id, int quantity)
    {
        var (exists, existingProductWeight) = await _productWeightService.CheckExistanceAsync(id);
        if (!exists) return;

        List<CartVM> Cart = await GetCart();
        CartVM? cartVm = Cart.Find(x => x.ProductWeightId == existingProductWeight!.Id)!;

        if (!_layoutService.CheckLoggedIn())
        {
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
        else
        {
            AppUser? user = await _userManager.FindByNameAsync(_accessor.HttpContext!.User.Identity!.Name!);

            if (cartVm != null)
            {
                var existingCartItem = await _cartRepository.GetAsync(x => x.User == user && x.ProductWeightId == cartVm.ProductWeightId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity = cartVm.Quantity + quantity;
                    await _cartRepository.UpdateAsync(existingCartItem);
                }
            }
            else
            {
                var newCartItem = new Cart
                {
                    Quantity = quantity,
                    ProductWeightId = existingProductWeight!.Id,
                    User = user!
                };

                await _cartRepository.CreateAsync(newCartItem);
            }
        }
    }

    public void ClearCookies()
    {
        if (_accessor.HttpContext!.Request.Cookies["Cart"] != null)
        {
            _accessor.HttpContext!.Response.Cookies.Delete("Cart");
        }
    }

    public async Task RemoveCartItem(int id)
    {
        List<CartVM> Cart = await GetCart();

        if (!_layoutService.CheckLoggedIn())
        {
            if (Cart != null && Cart.Count > 0)
            {
                var itemToRemove = Cart.FirstOrDefault(x => x.ProductWeightId == id);

                if (itemToRemove != null) Cart.Remove(itemToRemove);

                _accessor.HttpContext!.Response.Cookies.Append("Cart", JsonConvert.SerializeObject(Cart), new CookieOptions { Expires = DateTime.MaxValue });
            }
        }
        else
        {
            AppUser? user = await _userManager.FindByNameAsync(_accessor.HttpContext!.User.Identity!.Name!);

            if (Cart != null && Cart.Count > 0)
            {
                var existingCartItem = await _cartRepository.GetAsync(x => x.User == user && x.ProductWeightId == id);
                if (existingCartItem != null)
                {
                    await _cartRepository.DeleteAsync(existingCartItem.Id);
                }
            }
        }
    }

    public async Task<List<CartVM>> GetCart()
    {
        List<CartVM> cartVMs = new List<CartVM>();

        if (!_layoutService.CheckLoggedIn())
        {
            if (_accessor.HttpContext!.Request.Cookies["Cart"] != null)
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(_accessor.HttpContext!.Request.Cookies["Cart"]!)!;
            }
            return cartVMs;
        }
        else
        {
            AppUser? user = await _userManager.FindByNameAsync(_accessor.HttpContext!.User.Identity!.Name!);

            if (user != null)
            {
                List<Cart> cartList = await _cartRepository.GetAllAsync();
                if (cartList != null)
                {
                    foreach (var cart in cartList)
                    {
                        cartVMs.Add(cart);
                    }
                }
            }
            return cartVMs;
        }
    }

    public async Task MigrateToDBAsync()
    {
        List<CartVM> cartVM = new List<CartVM>();

        if (_accessor.HttpContext!.Request.Cookies["Cart"] != null)
        {
            cartVM = JsonConvert.DeserializeObject<List<CartVM>>(_accessor.HttpContext!.Request.Cookies["Cart"]!)!;
        }

        if (cartVM != null && cartVM.Count > 0)
        {
            AppUser? user = await _userManager.FindByNameAsync(_accessor.HttpContext!.User.Identity!.Name!);

            if (user != null)
            {
                foreach (var item in cartVM)
                {
                    Cart cart = new Cart
                    {
                        Quantity = item.Quantity,
                        ProductWeightId = item.ProductWeightId,
                        User = user
                    };
                    await _cartRepository.CreateAsync(cart);
                }
                ClearCookies();
            }
        }
    }
}
