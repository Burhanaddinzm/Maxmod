using Maxmod.Services.Implementations;
using Maxmod.Services.Interfaces;

namespace Maxmod.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IVendorService, VendorService>();

        return services;
    }
}
