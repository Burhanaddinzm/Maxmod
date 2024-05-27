using Maxmod.Areas.Admin.ViewModels.Order;
using Maxmod.Models;
using Maxmod.ViewModels.Pagination;
using System.Linq.Expressions;

namespace Maxmod.Services.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync(
       Expression<Func<Order, bool>>? where = null,
        string? order = null,
        string? orderByDesc = null,
        int? take = null,
        params string[] includes);
    List<Order> PaginateOrder(PagerVM pager, List<Order> orders);
    Task CreateOrderAsync(CreateOrderVM createOrderVM);
    Task<(bool, Order?)> CheckExistanceAsync(int id);
    Task UpdateOrderAsync(Order order);
}
