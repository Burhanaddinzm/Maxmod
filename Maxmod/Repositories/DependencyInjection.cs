using Maxmod.Repositories.Implementations;
using Maxmod.Repositories.Interfaces;

namespace Maxmod.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();

        return services;
    }
}
