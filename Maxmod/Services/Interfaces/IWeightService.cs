using Maxmod.Areas.Admin.ViewModels.Weight;
using Maxmod.Models;
using System.Linq.Expressions;

namespace Maxmod.Services.Interfaces;

public interface IWeightService
{
    Task<List<Weight>> GetAllWeightsAsync(Expression<Func<Weight, bool>>? expression = null, params string[] includes);
    Task CreateWeightAsync(CreateWeightVM createWeightVM);
    Task<(bool, Weight?)> CheckExistanceAsync(int id);
    Task<bool> CheckDuplicateAsync(string weightName, int? weightId = null);
    Task UpdateWeightAsync(UpdateWeightVM updateWeightVM, Weight Weight);
    Task DeleteWeightAsync(DeleteWeightVM deleteWeightVM);
}
