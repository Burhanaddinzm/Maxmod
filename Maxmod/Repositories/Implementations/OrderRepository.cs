using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories.Implementations;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
}
