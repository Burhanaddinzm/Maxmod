using Maxmod.Repositories.Implementations;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
