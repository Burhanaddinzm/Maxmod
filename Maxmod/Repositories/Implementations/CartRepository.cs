using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories.Implementations;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext context) : base(context)
    {
    }
}
