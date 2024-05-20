using Maxmod.Areas.Admin.ViewModels.ProductWeight;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;

namespace Maxmod.Services.Implementations;

public class ProductWeightService : IProductWeightService
{
    private readonly IProductWeightRepository _productWeightRepository;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ProductWeightService(
        IProductWeightRepository productWeightRepository,
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _productWeightRepository = productWeightRepository;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CheckDuplicateAsync(
    CreateProductWeightVM? createProductWeightVM = null,
    UpdateProductWeightVM? updateProductWeightVM = null)
    {
        ProductWeight? existingProductWeight;

        if (updateProductWeightVM != null)
        {
            existingProductWeight = await _productWeightRepository.GetAsync(
                x => x.Stock == updateProductWeightVM.Stock &&
                x.ProductId == updateProductWeightVM.ProductId &&
                x.WeightId == updateProductWeightVM.WeightId &&
                x.Price == updateProductWeightVM.Price &&
                x.DiscountPrice == updateProductWeightVM.DiscountPrice &&
                x.Id != updateProductWeightVM.Id
                );
        }
        else
        {
            existingProductWeight = await _productWeightRepository.GetAsync(
                x => x.Stock == createProductWeightVM!.Stock &&
                x.ProductId == createProductWeightVM!.ProductId &&
                x.WeightId == createProductWeightVM!.WeightId &&
                x.Price == createProductWeightVM!.Price &&
                x.DiscountPrice == createProductWeightVM!.DiscountPrice);
        }

        return existingProductWeight != null;
    }

    public async Task<(bool, ProductWeight?)> CheckExistanceAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

        ProductWeight? productWeight = await _productWeightRepository.GetAsync(id);

        if (productWeight == null)
            tempData["Error"] = "Product-Weight not found!";

        return (productWeight != null, productWeight);
    }

    public async Task CreateProductWeightAsync(CreateProductWeightVM createProductWeightVM)
    {
        var productWeight = new ProductWeight
        {
            Stock = createProductWeightVM.Stock,
            Price = createProductWeightVM.Price,
            DiscountPrice = createProductWeightVM.DiscountPrice,
            ProductId = createProductWeightVM.ProductId,
            WeightId = createProductWeightVM.WeightId,
        };
        await _productWeightRepository.CreateAsync(productWeight);
    }

    public async Task DeleteProductWeightAsync(DeleteProductWeightVM deleteProductWeightVM)
    {
        await _productWeightRepository.DeleteAsync(deleteProductWeightVM.Id);
    }

    public async Task<List<ProductWeight>> GetAllProductWeightsAsync(Expression<Func<ProductWeight, bool>>? expression = null, params string[] includes)
    {
        return await _productWeightRepository.GetAllAsync(expression, includes);
    }

    public async Task UpdateProductWeightAsync(UpdateProductWeightVM updateProductWeightVM, ProductWeight productWeight)
    {
        productWeight.Stock = updateProductWeightVM.Stock;
        productWeight.Price = updateProductWeightVM.Price;
        productWeight.DiscountPrice = updateProductWeightVM.DiscountPrice;
        productWeight.ProductId = updateProductWeightVM.ProductId;
        productWeight.WeightId = updateProductWeightVM.WeightId;

        await _productWeightRepository.UpdateAsync(productWeight);
    }
}
