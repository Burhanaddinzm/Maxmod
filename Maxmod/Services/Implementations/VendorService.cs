using Maxmod.Models;
using Maxmod.Repositories.Implementations;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using System.Linq.Expressions;

namespace Maxmod.Services.Implementations;

public class VendorService : IVendorService
{
    readonly IVendorRepository _vendorRepository;

    public VendorService(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task CreateVendorAsync(AppUser user)
    {
        Vendor vendor = new Vendor
        {
            Name = "Vendor_" + user.Email,
            Image = "2_1c4b30d7-c150-41db-8703-2e4d065c8cbe.png",
            User = user,
            UserId = user.Id,
        };

        await _vendorRepository.CreateAsync(vendor);
    }

    public async Task<Vendor> GetVendorAsync(int id)
    {
        return await _vendorRepository.GetAsync(id);
    }

    public async Task<List<Vendor>> GetAllVendorsAsync(Expression<Func<Vendor, bool>>? expression = null, params string[] includes)
    {
        return await _vendorRepository.GetAllAsync(expression, includes);
    }

}
