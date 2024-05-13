using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories.Implementations;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}
