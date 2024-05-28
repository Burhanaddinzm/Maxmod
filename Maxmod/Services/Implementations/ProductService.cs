using Maxmod.Areas.Admin.ViewModels.Product;
using Maxmod.Extensions;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IWebHostEnvironment _env;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWeightService _weightService;

    public ProductService(
        IProductRepository productRepository,
        IWebHostEnvironment env,
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IHttpContextAccessor httpContextAccessor,
        IWeightService weightService)
    {
        _productRepository = productRepository;
        _env = env;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
        _weightService = weightService;
    }

    public async Task<List<Product>> GetAllProductsAsync(
       Expression<Func<Product, bool>>? where = null,
        string? order = null,
        string? orderByDesc = null,
        int? take = null,
        params string[] includes)
    {
        return await _productRepository.GetAllAsync(where, order, orderByDesc, take, includes);
    }

    public List<Product> PaginateProduct(PagerVM pager, List<Product> products)
    {
        int itemsToSkip = (pager.CurrentPage - 1) * pager.PageSize;

        return products.Skip(itemsToSkip).Take(pager.PageSize).ToList();
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

        product.ProductWeights.Add(new ProductWeight
        {
            Stock = 0,
            Price = 0,
            DiscountPrice = 0,
            Product = product,
            Weight = await _weightService.GetWeightByNameAsync("Default")
        });

        await _productRepository.CreateAsync(product);
        return null;
    }

    public async Task<FileValidationResult?> UpdateProductAsync(UpdateProductVM updateProductVM, Product product)
    {
        var productImages = new List<ProductImage>();

        if (updateProductVM.AdditionalImages != null)
        {
            var additionalValidationResult = await ValidateAndCreateImageAsync(updateProductVM.AdditionalImages, productImages);
            if (!additionalValidationResult.IsValid) return additionalValidationResult;
        }
        if (updateProductVM.MainImage != null)
        {
            var mainValidationResult = await ValidateAndCreateImageAsync(new List<IFormFile> { updateProductVM.MainImage }, productImages);
            if (!mainValidationResult.IsValid) return mainValidationResult;
            product.ProductImages!.FirstOrDefault(x => x.IsMain)!.IsDeleted = true;

        }
        if (updateProductVM.HoverImage != null)
        {
            var hoverValidationResult = await ValidateAndCreateImageAsync(new List<IFormFile> { updateProductVM.HoverImage }, productImages);
            product.ProductImages!.FirstOrDefault(x => x.IsHover)!.IsDeleted = true;
            if (!hoverValidationResult.IsValid) return hoverValidationResult;
        }

        product.Name = updateProductVM.Name;
        product.Description = updateProductVM.Description;
        product.CategoryId = updateProductVM.CategoryId;
        product.VendorId = updateProductVM.VendorId;

        if (product.ProductImages == null)
        {
            product.ProductImages = new List<ProductImage>();
        }
        if (productImages.Count > 0)
        {
            foreach (var image in productImages)
            {
                product.ProductImages.Add(image);
            }
        }

        await _productRepository.UpdateAsync(product);
        return null;
    }

    public async Task DeleteProductAsync(DeleteProductVM deleteProductVM)
    {
        await _productRepository.DeleteAsync(deleteProductVM.Id);
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

        Product? product = await _productRepository.GetAsync(x => x.Id == id, "ProductImages", "Category", "Vendor.User", "ProductWeights.Weight");

        if (product == null)
            tempData["Error"] = "Product not found!";

        return (product != null, product);
    }

    public List<Product> ApplyInStockFilter(List<Product> products, string? inStock)
    {
        return inStock switch
        {
            "true" => products.Where(p => p.ProductWeights.Any(w => w.Stock > 0)).ToList(),
            "false" => products.Where(p => p.ProductWeights.All(w => w.Stock == 0)).ToList(),
            "both" => products,
            _ => products
        };
    }

    public async Task<List<Product>> FetchClientProductsAsync(string? category, string? orderBy, string? orderByDesc, string? searchString, List<string>? weightFilters)
    {
        Expression<Func<Product, bool>>? filterExpression = null;

        if (!string.IsNullOrEmpty(searchString))
        {
            filterExpression = category != null
                ? x => (x.Category.Name == category || x.Category.Parent.Name == category) && x.Name.ToLower().Contains(searchString.Trim().ToLower())
                : x => x.Name.ToLower().Contains(searchString.Trim().ToLower());
        }
        else if (category != null)
        {
            filterExpression = x => x.Category.Name == category || x.Category.Parent.Name == category;
        }

        var products = await GetAllProductsAsync(filterExpression, orderBy, orderByDesc, null, "ProductWeights.Weight", "Vendor", "ProductImages");

        if (weightFilters != null && weightFilters.Any())
        {
            products = products.Where(p => p.ProductWeights.Any(pw => weightFilters.Contains(pw.Weight.Name))).ToList();
        }

        return products;
    }

}
