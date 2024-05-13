using Maxmod.Extensions;
using Maxmod.Models;
using Maxmod.Repositories.Implementations;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Implementations;

public class ProductService : IProductService
{
    readonly IProductRepository _productRepository;
    readonly IWebHostEnvironment _env;

    public ProductService(IProductRepository productRepository, IWebHostEnvironment env)
    {
        _productRepository = productRepository;
        _env = env;
    }

    public async Task<List<Product>> GetAllProductsAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes)
    {
        return await _productRepository.GetAllAsync(expression, includes);
    }

    public async Task<FileValidationResult> ValidateAndCreateImageAsync(List<IFormFile> files, List<ProductImage> productImages)
    {
        foreach (var file in files)
        {
            var validationResult = file.ValidateFile();
            if (!validationResult.IsValid)
                return validationResult;

            productImages.Add(await CreateProductImageAsync(file, file.Name == "MainImage", file.Name == "HoverImage"));
        }
        return new FileValidationResult(true);
    }

    public async Task<ProductImage> CreateProductImageAsync(IFormFile file, bool isMain, bool isHover)
    {
        string fileName = await file.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "product");

        ProductImage productImage = new ProductImage
        {
            Url = fileName,
            IsMain = isMain,
            IsHover = isHover
        };

        return productImage;
    }
}
