using Maxmod.Models;
using System.Linq.Expressions;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes);
    Task<FileValidationResult> ValidateAndCreateImageAsync(List<IFormFile> files, List<ProductImage> productImages);
    Task<ProductImage> CreateProductImageAsync(IFormFile file, bool isMain, bool isHover);
}
