using Braintree;
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

    public CheckoutController(IBraintreeService braintreeService, ICartService cartService, IHttpContextAccessor accessor, UserManager<AppUser> userManager, IProductWeightService productWeightService)
    {
        _braintreeService = braintreeService;
        _cartService = cartService;
        _accessor = accessor;
        _userManager = userManager;
        _productWeightService = productWeightService;
    }

    public async Task<IActionResult> Index()
    {
        var gateway = _braintreeService.GetGateway();
        var clientToken = gateway.ClientToken.Generate();
        ViewBag.ClientToken = clientToken;

        var user = await _userManager.FindByNameAsync(_accessor.HttpContext!.User.Identity!.Name!);

        var cartList = await _cartService.GetCart();
        var productWeights = await _productWeightService.GetAllProductWeightsAsync();

        var data = new PurchaseVM
        {
            Quantity = cartList.Select(x => x.Quantity).ToList(),
            ProductWeightId = cartList.Select(x => x.ProductWeightId).ToList(),
            UserId = user!.Id,
            Nonce = "fake-valid-nonce"
        };

        foreach (var cart in cartList)
        {
            var productWeight = productWeights.Find(x => x.Id == cart.ProductWeightId);
            if (productWeight != null)
            {
                if (productWeight.DiscountPrice != 0)
                {
                    data.TotalPrice.Add(productWeight.DiscountPrice);
                }
                else
                {
                    data.TotalPrice.Add(productWeight.Price);
                }
            }
        }
        return View(data);
    }

    [HttpPost]
    public IActionResult Create(PurchaseVM model)
    {
        var gateway = _braintreeService.GetGateway();
        var request = new TransactionRequest
        {
            Amount = model.TotalPrice.Sum(),
            PaymentMethodNonce = model.Nonce,
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true
            }
        };

        Result<Transaction> result = gateway.Transaction.Sale(request);

        if (result.IsSuccess())
        {
            TempData["Notification"] = "Success!";
            return View("Notification");
        }
        else
        {
            TempData["Notification"] = "Failure!";
            return View("Notification");
        }
    }
}

