using Maxmod.Models;

namespace Maxmod.Services.Interfaces;

public interface IProductImageService
{
    Task<List<ProductImage>?> GetProductImagesAsync(int id);
    Task<bool> DeleteProductImageAsync(int id);
}
