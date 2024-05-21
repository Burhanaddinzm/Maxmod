using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Maxmod.Repositories.Implementations;

public class SettingsRepository : Repository<Settings>, ISettingsRepository
{
    public SettingsRepository(AppDbContext context) : base(context)
    {
    }
}
