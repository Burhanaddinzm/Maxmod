using Maxmod.Areas.Admin.ViewModels.Order;
using Maxmod.Models;
using Maxmod.Repositories.Implementations;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;

namespace Maxmod.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _accessor;

    public OrderService(IOrderRepository orderRepository, UserManager<AppUser> userManager, IHttpContextAccessor accessor)
    {
        _orderRepository = orderRepository;
        _userManager = userManager;
        _accessor = accessor;
    }

    public async Task<List<Order>> GetAllOrdersAsync(
       Expression<Func<Order, bool>>? where = null,
        string? order = null,
        string? orderByDesc = null,
        int? take = null,
        params string[] includes)
    {
        AppUser? user = await _userManager.FindByNameAsync(_accessor.HttpContext!.User.Identity!.Name!);
        return await _orderRepository.GetAllAsync(where, order, orderByDesc, take, includes);
    }

    public List<Order> PaginateOrder(PagerVM pager, List<Order> orders)
    {
        int itemsToSkip = (pager.CurrentPage - 1) * pager.PageSize;

        return orders.Skip(itemsToSkip).Take(pager.PageSize).ToList();
    }
    public async Task CreateOrderAsync(CreateOrderVM createOrderVM)
    {
        var order = new Order
        {
            Quantity = createOrderVM.Quantity,
            UserId = createOrderVM.UserId,
            ProductWeightId = createOrderVM.ProductWeightId,
            TotalPrice = createOrderVM.TotalPrice,
        };

        await _orderRepository.CreateAsync(order);
    }

    public async Task<(bool, Order?)> CheckExistanceAsync(int id)
    {
        var httpContext = _accessor.HttpContext;

        Order? order = await _orderRepository.GetAsync(id);
        return (order != null, order);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        await _orderRepository.UpdateAsync(order);
    }
}
