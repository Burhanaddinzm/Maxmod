using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories.Implementations;

public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(AppDbContext context) : base(context)
    {
    }
}
