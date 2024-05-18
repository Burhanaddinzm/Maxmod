using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Maxmod.Services.Implementations;

public class ProductImageService : IProductImageService
{
    readonly IProductImageRepository _repository;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductImageService(IProductImageRepository repository, ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<ProductImage>?> GetProductImagesAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);
        var productImages = await _repository.GetAllAsync(x => x.ProductId == id);
        if (productImages != null)
        {
            return productImages;
        }
        else
        {
            tempData["Error"] = "Product Image not found!";
            return null;
        }
    }

    public async Task<bool> DeleteProductImageAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);
        var productImage = await _repository.GetAsync(id);
        if (productImage == null)
        {
            tempData["Error"] = "Product Image not found!";
            return false;
        }

        await _repository.DeleteAsync(id);
        return true;
    }
}
