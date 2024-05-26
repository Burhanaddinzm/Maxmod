using Maxmod.Areas.Admin.ViewModels.ProductWeight;
using Maxmod.Areas.Admin.ViewModels.Weight;
using Maxmod.Models;
using Maxmod.ViewModels.Pagination;
using System.Linq.Expressions;

namespace Maxmod.Services.Interfaces;

public interface IProductWeightService
{
    Task<bool> CheckDuplicateAsync(
        CreateProductWeightVM? createProductWeightVM = null,
        UpdateProductWeightVM? updateProductWeightVM = null);
    Task<(bool, ProductWeight?)> CheckExistanceAsync(int id);
    Task CreateProductWeightAsync(CreateProductWeightVM createProductWeightVM);
    Task DeleteProductWeightAsync(DeleteProductWeightVM deleteProductWeightVM);
    Task<List<ProductWeight>> GetAllProductWeightsAsync(
       Expression<Func<ProductWeight, bool>>? where = null,
        string? order = null,
        string? orderByDesc = null,
        int? take = null,
        params string[] includes);
    Task UpdateProductWeightAsync(UpdateProductWeightVM updateProductWeightVM, ProductWeight productWeight);
    List<ProductWeight> PaginateProductWeight(PagerVM pager, List<ProductWeight> productWeights);
}
