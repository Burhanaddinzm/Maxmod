using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories.Implementations;

public class WeightRepository : Repository<Weight>, IWeightRepository
{
    public WeightRepository(AppDbContext context) : base(context)
    {
    }
}
