using Maxmod.Areas.Admin.ViewModels.Weight;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;

namespace Maxmod.Services.Implementations;

public class WeightService : IWeightService
{
    private readonly IWeightRepository _weightRepository;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeightService(
        IWeightRepository weightRepository,
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _weightRepository = weightRepository;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CheckDuplicateAsync(string weightName, int? weightId = null)
    {
        Weight? existingWeight;

        if (weightId != null)
        {
            existingWeight = await _weightRepository.GetAsync(
                x => x.Name.Trim().ToLower() == weightName.Trim().ToLower() &&
                x.Id != weightId
                );
        }
        else existingWeight = await _weightRepository.GetAsync(x => x.Name.Trim().ToLower() == weightName.Trim().ToLower());

        return existingWeight != null;
    }

    public async Task<(bool, Weight?)> CheckExistanceAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

        Weight? weight = await _weightRepository.GetAsync(id);

        if (weight == null)
            tempData["Error"] = "Weight not found!";

        return (weight != null, weight);
    }

    public async Task CreateWeightAsync(CreateWeightVM createWeightVM)
    {
        await _weightRepository.CreateAsync(new Weight { Name = createWeightVM.Name });
    }

    public async Task DeleteWeightAsync(DeleteWeightVM deleteWeightVM)
    {
        await _weightRepository.DeleteAsync(deleteWeightVM.Id);
    }

    public async Task<List<Weight>> GetAllWeightsAsync(
       Expression<Func<Weight, bool>>? where = null,
       Expression<Func<Weight, object>>? order = null,
       int? take = null,
       params string[] includes)
    {
        return await _weightRepository.GetAllAsync(where,order,take, includes);
    }

    public async Task UpdateWeightAsync(UpdateWeightVM updateWeightVM, Weight Weight)
    {
        Weight.Name = updateWeightVM.Name;
        await _weightRepository.UpdateAsync(Weight);
    }
}
