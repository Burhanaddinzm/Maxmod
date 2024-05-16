using Maxmod.Areas.Admin.ViewModels.Vendor;
using Maxmod.Models;
using System.Linq.Expressions;

namespace Maxmod.Services.Interfaces;

public interface IVendorService
{
    Task CreateVendorAsync(AppUser user);
    Task<Vendor> GetVendorAsync(int id);
    Task<List<Vendor>> GetAllVendorsAsync(Expression<Func<Vendor, bool>>? expression = null, params string[] includes);
    Task<(bool, Vendor?)> CheckExistanceAsync(int id);
    Task<bool> CheckDuplicateAsync(string vendorName, int vendorId);
    Task UpdateVendorAsync(UpdateVendorVM updateVendorVM, Vendor vendor);
    Task DeleteVendorAsync(DeleteVendorVM deleteVendorVM);
    Task AcceptVendor(Vendor vendor);
    Task RejectVendor(Vendor vendor);
}
