using Maxmod.Areas.Admin.ViewModels.Product;
using Maxmod.Extensions;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Implementations;

public class ProductService : IProductService
{
    readonly IProductRepository _productRepository;
    readonly IWebHostEnvironment _env;
    readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    readonly IHttpContextAccessor _httpContextAccessor;

    public ProductService(
        IProductRepository productRepository,
        IWebHostEnvironment env,
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _productRepository = productRepository;
        _env = env;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Product>> GetAllProductsAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes)
    {
        return await _productRepository.GetAllAsync(expression, includes);
    }

    public async Task<FileValidationResult?> CreateProductAsync(CreateProductVM createProductVM)
    {
        var productImages = new List<ProductImage>();

        if (createProductVM.AdditionalImages != null)
        {
            var additionalValidationResult = await ValidateAndCreateImageAsync(createProductVM.AdditionalImages, productImages);
            if (!additionalValidationResult.IsValid) return additionalValidationResult;
        }

        var mainValidationResult = await ValidateAndCreateImageAsync(new List<IFormFile> { createProductVM.MainImage }, productImages);
        if (!mainValidationResult.IsValid) return mainValidationResult;

        var hoverValidationResult = await ValidateAndCreateImageAsync(new List<IFormFile> { createProductVM.HoverImage }, productImages);
        if (!hoverValidationResult.IsValid) return hoverValidationResult;

        Product product = new Product
        {
            Name = createProductVM.Name,
            Description = createProductVM.Description,
            ProductImages = productImages,
            VendorId = createProductVM.VendorId,
            CategoryId = createProductVM.CategoryId
        };

        await _productRepository.CreateAsync(product);
        return null;
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

    public async Task<bool> CheckDuplicateAsync(string productName, int? productId = null)
    {
        Product? existingProduct;

        if (productId != null)
        {
            existingProduct = await _productRepository.GetAsync(
                x => x.Name.Trim().ToLower() == productName.Trim().ToLower() &&
                x.Id != productId
                );
        }
        else existingProduct = await _productRepository.GetAsync(x => x.Name.Trim().ToLower() == productName.Trim().ToLower());

        return existingProduct != null;
    }

    public async Task<(bool, Product?)> CheckExistanceAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

        Product? product = await _productRepository.GetAsync(id);

        if (product == null)
            tempData["Error"] = "Product not found!";

        return (product != null, product);
    }
}
