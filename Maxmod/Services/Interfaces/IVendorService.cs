﻿using Maxmod.Areas.Admin.ViewModels.Vendor;
using Maxmod.Models;
using Maxmod.ViewModels.Pagination;
using System.Linq.Expressions;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Interfaces;

public interface IVendorService
{
    Task CreateVendorAsync(AppUser user);
    Task<Vendor> GetVendorAsync(int id);
    Task<List<Vendor>> GetAllVendorsAsync(
       Expression<Func<Vendor, bool>>? where = null,
        string? order = null,
        string? orderByDesc = null,
        int? take = null,
        params string[] includes);
    Task<(bool, Vendor?)> CheckExistanceAsync(int id);
    Task<bool> CheckDuplicateAsync(string vendorName, int vendorId);
    Task<FileValidationResult?> UpdateVendorAsync(UpdateVendorVM updateVendorVM, Vendor vendor);
    Task DeleteVendorAsync(DeleteVendorVM deleteVendorVM);
    List<Vendor> PaginateVendor(PagerVM pager, List<Vendor> vendors);
    Task AcceptVendor(Vendor vendor);
    Task RejectVendor(Vendor vendor);
}
