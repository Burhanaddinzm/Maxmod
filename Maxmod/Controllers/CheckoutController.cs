using Braintree;
using Maxmod.Areas.Admin.ViewModels.Order;
using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Checkout;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class CheckoutController : Controller
{
    private readonly IBraintreeService _braintreeService;
    private readonly ICartService _cartService;
    private readonly IHttpContextAccessor _accessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductWeightService _productWeightService;
    private readonly IOrderService _orderService;
    private readonly ILayoutService _layoutService;

    public CheckoutController(
        IBraintreeService braintreeService,
        ICartService cartService,
        IHttpContextAccessor accessor,
        UserManager<AppUser> userManager,
        IProductWeightService productWeightService,
        IOrderService orderService,
        ILayoutService layoutService)
    {
        _braintreeService = braintreeService;
        _cartService = cartService;
        _accessor = accessor;
        _userManager = userManager;
        _productWeightService = productWeightService;
        _orderService = orderService;
        _layoutService = layoutService;
    }

    public async Task<IActionResult> Index()
    {
        if (!_layoutService.CheckLoggedIn())
        {
            return RedirectToAction("Index", "Error");
        }

        var gateway = _braintreeService.GetGateway();
        var clientToken = gateway.ClientToken.Generate();
        ViewBag.ClientToken = clientToken;

        var user = await _userManager.FindByNameAsync(_accessor.HttpContext!.User.Identity!.Name!);

        var cartList = await _cartService.GetCart();
        var productWeights = await _productWeightService.GetAllProductWeightsAsync();

        var data = new PurchaseVM
        {
            UserId = user!.Id,
            Nonce = ""
        };

        var consolidatedCart = new Dictionary<int, (int Quantity, decimal TotalPrice)>();

        foreach (var cart in cartList)
        {
            var productWeight = productWeights.Find(x => x.Id == cart.ProductWeightId);
            if (productWeight != null)
            {
                decimal price = productWeight.DiscountPrice != 0 ? productWeight.DiscountPrice : productWeight.Price;

                if (consolidatedCart.ContainsKey(cart.ProductWeightId))
                {
                    consolidatedCart[cart.ProductWeightId] = (
                        Quantity: consolidatedCart[cart.ProductWeightId].Quantity + cart.Quantity,
                        TotalPrice: consolidatedCart[cart.ProductWeightId].TotalPrice + (price * cart.Quantity)
                    );
                }
                else
                {
                    consolidatedCart[cart.ProductWeightId] = (
                        Quantity: cart.Quantity,
                        TotalPrice: price * cart.Quantity
                    );
                }
            }
        }

        foreach (var item in consolidatedCart)
        {
            data.ProductWeightId.Add(item.Key);
            data.Quantity.Add(item.Value.Quantity);
            data.TotalPrice.Add(item.Value.TotalPrice);
        }
        return View(data);
    }


    [HttpPost]
    public async Task<IActionResult> Create(PurchaseVM purchaseVM)
    {
        var gateway = _braintreeService.GetGateway();
        var request = new TransactionRequest
        {
            Amount = purchaseVM.TotalPrice.Sum(),
            PaymentMethodNonce = purchaseVM.Nonce,
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true
            }
        };

        Result<Transaction> result = gateway.Transaction.Sale(request);

        if (purchaseVM != null && purchaseVM.ProductWeightId.Count > 0)
        {
            for (int i = 0; i < purchaseVM.ProductWeightId.Count; i++)
            {
                var order = new CreateOrderVM
                {
                    ProductWeightId = purchaseVM.ProductWeightId[i],
                    TotalPrice = purchaseVM.TotalPrice[i],
                    Quantity = purchaseVM.Quantity[i],
                    UserId = purchaseVM.UserId,
                };
                await _orderService.CreateOrderAsync(order);
            }

            var cart = await _cartService.GetCart();
            foreach (var item in cart)
            {
                await _cartService.RemoveCartItem(item.ProductWeightId);
            }
        }

        if (result.IsSuccess())
        {
            TempData["Notification"] = "Payment Successfull!";
            return RedirectToAction("Index", "Notification");
        }
        else
        {
            TempData["Notification"] = "Payment Failed!";
            return RedirectToAction("Index", "Notification");
        }
    }
}

