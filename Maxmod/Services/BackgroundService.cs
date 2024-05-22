using Maxmod.Repositories.Interfaces;

namespace Maxmod.Services;

public class BackgroundService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(ChangeProductStatus, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private async void ChangeProductStatus(object? state)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

            var products = await productRepository.GetAllAsync(x => x.CreatedAt.AddDays(7) < DateTime.UtcNow.AddHours(4));
            foreach (var product in products)
            {
                product.IsNew = false;
                await productRepository.UpdateAsync(product);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
