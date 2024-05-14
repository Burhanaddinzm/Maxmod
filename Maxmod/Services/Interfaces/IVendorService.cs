using Maxmod.Models;
using System.Linq.Expressions;

namespace Maxmod.Services.Interfaces;

public interface IVendorService
{
    Task CreateVendorAsync(AppUser user);
    Task<Vendor> GetVendorAsync(int id);
    Task<List<Vendor>> GetAllVendorsAsync(Expression<Func<Vendor, bool>>? expression = null, params string[] includes);
}
