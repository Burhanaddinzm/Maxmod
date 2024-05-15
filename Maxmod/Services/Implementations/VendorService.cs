using Maxmod.Enums;
using Maxmod.Models;
using Maxmod.Repositories.Implementations;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;

namespace Maxmod.Services.Implementations;

public class VendorService : IVendorService
{
    readonly IVendorRepository _vendorRepository;
    readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly UserManager<AppUser> _userManager;

    public VendorService(
        IVendorRepository vendorRepository,
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager)
    {
        _vendorRepository = vendorRepository;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)
    {
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential("maxmodteam@outlook.com", "Maxmod123")
    };

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

    public async Task<(bool, Vendor?)> CheckExistanceAsync(int id)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

        Vendor? vendor = await _vendorRepository.GetAsync(x => x.Id == id, "User");

        if (vendor == null)
            tempData["Error"] = "Vendor not found!";

        return (vendor != null, vendor);
    }

    public async Task AcceptVendor(Vendor vendor)
    {
        vendor.IsConfirmed = true;
        await _userManager.RemoveFromRoleAsync(vendor.User, Roles.Customer.ToString());
        await _userManager.AddToRoleAsync(vendor.User, Roles.Vendor.ToString());
        await _vendorRepository.UpdateAsync(vendor);

        MailMessage mailMessage = new MailMessage()
        {
            Subject = "Maxmod Vendor Request",
            From = new MailAddress("maxmodteam@outlook.com"),
            Body = @"Your vendor request has been accepted.
<br>
<a href='https://localhost:7095/'>Maxmod</a>",
            IsBodyHtml = true,
        };
        mailMessage.To.Add(vendor.User.Email!);
        smtpClient.Send(mailMessage);
    }

    public async Task RejectVendor(Vendor vendor)
    {
        await _vendorRepository.DeleteAsync(vendor.Id);

        MailMessage mailMessage = new MailMessage()
        {
            Subject = "Maxmod Vendor Request",
            From = new MailAddress("maxmodteam@outlook.com"),
            Body = @"Your vendor request has been declined.
<br>
<a href='https://localhost:7095/'>Maxmod</a>",
            IsBodyHtml = true,
        };
        mailMessage.To.Add(vendor.User.Email!);
        smtpClient.Send(mailMessage);
    }
}
