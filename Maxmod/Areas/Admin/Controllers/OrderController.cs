using Maxmod.Models;
using Maxmod.Services.Implementations;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Areas.Admin.Controllers;
[Authorize(Roles = "Admin,Vendor")]
[Area("Admin")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var orders = new List<Order>();

        if (User.IsInRole("Vendor"))
        {
            orders = await _orderService.GetAllOrdersAsync(x => x.ProductWeight.Product.Vendor.User.UserName == User.Identity!.Name && !x.IsComplete,
                null, null, null, "User", "ProductWeight.Product", "ProductWeight.Weight");
        }
        else
        {
            orders = await _orderService.GetAllOrdersAsync(x => !x.IsComplete, null, null, null, "User", "ProductWeight.Product", "ProductWeight.Weight");
        }

        const int pageSize = 10;
        if (page < 1) page = 1;

        int itemCount = orders.Count();

        var pager = new PagerVM(itemCount, page, pageSize);

        var data = _orderService.PaginateOrder(pager, orders);

        ViewBag.Pager = pager;

        return View(data);
    }

    public async Task<IActionResult> Confirm(int id)
    {
        var (exists, order) = await _orderService.CheckExistanceAsync(id);
        if (!exists)
        {
            TempData["Error"] = "Order not found!";
            return RedirectToAction("Index", "Error");
        }

        order!.IsComplete = true;
        order!.IsDeleted = true;
        await _orderService.UpdateOrderAsync(order);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Reject(int id)
    {
        var (exists, order) = await _orderService.CheckExistanceAsync(id);
        if (!exists)
        {
            TempData["Error"] = "Order not found!";
            return RedirectToAction("Index", "Error");
        }

        order!.IsDeleted = true;
        await _orderService.UpdateOrderAsync(order);
        return RedirectToAction("Index");
    }
}
