using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories.Implementations;

public class ProductWeightRepository : Repository<ProductWeight>, IProductWeightRepository
{
    public ProductWeightRepository(AppDbContext context) : base(context)
    {
    }
}
