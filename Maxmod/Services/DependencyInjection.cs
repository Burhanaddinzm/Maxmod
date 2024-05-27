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
        services.AddScoped<IProductImageService, ProductImageService>();
        services.AddScoped<ILayoutService, LayoutService>();
        services.AddScoped<IWeightService, WeightService>();
        services.AddScoped<IProductWeightService, ProductWeightService>();
        services.AddScoped<ISettingsService, SettingsService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddTransient<IBraintreeService, BraintreeService>();
        services.AddHostedService<BackgroundService>();

        return services;
    }
}
