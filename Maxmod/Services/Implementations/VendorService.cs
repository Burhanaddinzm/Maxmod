using Maxmod.Areas.Admin.ViewModels.Vendor;
using Maxmod.Enums;
using Maxmod.Extensions;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Implementations;

public class VendorService : IVendorService
{
    private readonly IVendorRepository _vendorRepository;
    private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public VendorService(
        IVendorRepository vendorRepository,
        ITempDataDictionaryFactory tempDataDictionaryFactory,
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        IWebHostEnvironment env,
        IConfiguration configuration)
    {
        _vendorRepository = vendorRepository;
        _tempDataDictionaryFactory = tempDataDictionaryFactory;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _env = env;
        _configuration = configuration;
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

    public async Task<List<Vendor>> GetAllVendorsAsync(
      Expression<Func<Vendor, bool>>? where = null,
        string? order = null,
        string? orderByDesc = null,
        int? take = null,
        params string[] includes)
    {
        return await _vendorRepository.GetAllAsync(where, order, orderByDesc, take, includes);
    }

    public List<Vendor> PaginateVendor(PagerVM pager, List<Vendor> vendors)
    {
        int itemsToSkip = (pager.CurrentPage - 1) * pager.PageSize;

        return vendors.Skip(itemsToSkip).Take(pager.PageSize).ToList();
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

    public async Task<bool> CheckDuplicateAsync(string vendorName, int vendorId)
    {
        Vendor? existingVendor = await _vendorRepository.GetAsync(
            x => x.Name.Trim().ToLower() == vendorName.Trim().ToLower() &&
            x.Id != vendorId
            );

        return existingVendor != null;
    }

    public async Task<FileValidationResult?> UpdateVendorAsync(UpdateVendorVM updateVendorVM, Vendor vendor)
    {
        if (updateVendorVM.Image != null)
        {
            var validationResult = updateVendorVM.Image.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateVendorVM.Image.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "vendor");
            if (vendor.Image != "2_1c4b30d7-c150-41db-8703-2e4d065c8cbe.png")
                updateVendorVM.Image.DeleteFile(_env.WebRootPath, "client", "assets", "images", "vendor", vendor.Image);

            vendor.Image = filename;
        }
        vendor.Name = updateVendorVM.Name;

        await _vendorRepository.UpdateAsync(vendor);
        return null;
    }

    public async Task DeleteVendorAsync(DeleteVendorVM deleteVendorVM)
    {
        AppUser? user = await _userManager.FindByIdAsync(deleteVendorVM.UserId);

        await _vendorRepository.DeleteAsync(deleteVendorVM.Id);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, Roles.Vendor.ToString());
            await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());
        }
    }

    public async Task AcceptVendor(Vendor vendor)
    {
        SmtpClient smtpClient = new SmtpClient(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]))
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"])
        };
        MailMessage mailMessage = new MailMessage()
        {
            Subject = "Maxmod Vendor Request",
            From = new MailAddress(_configuration["EmailSettings:Email"]),
            IsBodyHtml = true,
        };

        vendor.IsConfirmed = true;
        await _userManager.RemoveFromRoleAsync(vendor.User, Roles.Customer.ToString());
        await _userManager.AddToRoleAsync(vendor.User, Roles.Vendor.ToString());
        await _vendorRepository.UpdateAsync(vendor);

        mailMessage.Body = @"<!doctypehtml><html dir=ltr xmlns=http://www.w3.org/1999/xhtml xmlns:o=urn:schemas-microsoft-com:office:office><meta charset=UTF-8><meta content=""width=device-width,initial-scale=1""name=viewport><meta name=x-apple-disable-message-reformatting><meta content=""IE=edge""http-equiv=X-UA-Compatible><meta content=""telephone=no""name=format-detection><title></title><style>@import url(https://fonts.googleapis.com/css2?family=Imprima&display=swap);html *{font-family:Imprima,sans-serif!important}#outlook a{padding:0}.es-button{mso-style-priority:100!important;text-decoration:none!important}a[x-apple-data-detectors]{color:inherit!important;text-decoration:none!important;font-size:inherit!important;font-family:inherit!important;font-weight:inherit!important;line-height:inherit!important}.es-desk-hidden{display:none;float:left;overflow:hidden;width:0;max-height:0;line-height:0;mso-hide:all}body{width:100%;font-family:Imprima,Arial,sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table{mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0}.es-wrapper,body,table td{padding:0;margin:0}.es-content,.es-footer,.es-header{table-layout:fixed!important;width:100%}img{display:block;border:0;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}hr,p{margin:0}h1,h2,h3,h4,h5{margin:0;line-height:120%;mso-line-height-rule:exactly;font-family:Imprima,Arial,sans-serif}a,ol li,p,ul li{-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly}.es-left{float:left}.es-right{float:right}.es-p5{padding:5px}.es-p5t{padding-top:5px}.es-p5b{padding-bottom:5px}.es-p5l{padding-left:5px}.es-p5r{padding-right:5px}.es-p10{padding:10px}.es-p10t{padding-top:10px}.es-p10b{padding-bottom:10px}.es-p10l{padding-left:10px}.es-p10r{padding-right:10px}.es-p15{padding:15px}.es-p15t{padding-top:15px}.es-p15b{padding-bottom:15px}.es-p15l{padding-left:15px}.es-p15r{padding-right:15px}.es-p20{padding:20px}.es-p20t{padding-top:20px}.es-p20b{padding-bottom:20px}.es-p20l{padding-left:20px}.es-p20r{padding-right:20px}.es-p25{padding:25px}.es-p25t{padding-top:25px}.es-p25b{padding-bottom:25px}.es-p25l{padding-left:25px}.es-p25r{padding-right:25px}.es-p30{padding:30px}.es-p30t{padding-top:30px}.es-p30b{padding-bottom:30px}.es-p30l{padding-left:30px}.es-p30r{padding-right:30px}.es-p35{padding:35px}.es-p35t{padding-top:35px}.es-p35b{padding-bottom:35px}.es-p35l{padding-left:35px}.es-p35r{padding-right:35px}.es-p40{padding:40px}.es-p40t{padding-top:40px}.es-p40b{padding-bottom:40px}.es-p40l{padding-left:40px}.es-p40r{padding-right:40px}.es-menu td{border:0}.es-menu td a img{display:inline-block!important;vertical-align:middle}s{text-decoration:line-through}ol li,p,ul li{font-family:Imprima,Arial,sans-serif;line-height:150%}ol li,ul li{margin-bottom:15px;margin-left:0}a{text-decoration:underline}.es-menu td a{text-decoration:none;display:block;font-family:Imprima,Arial,sans-serif}.es-wrapper{width:100%;height:100%;background-repeat:repeat;background-position:center top}.es-wrapper,.es-wrapper-color{background-color:#fff}.es-header{background-color:transparent;background-repeat:repeat;background-position:center top}.es-header-body{background-color:#efefef}.es-header-body ol li,.es-header-body p,.es-header-body ul li{color:#2d3142;font-size:14px}.es-header-body a{color:#2d3142;font-size:14px}.es-content-body{background-color:#efefef}.es-content-body ol li,.es-content-body p,.es-content-body ul li{color:#2d3142;font-size:18px}.es-content-body a{color:#2d3142;font-size:18px}.es-footer{background-color:transparent;background-repeat:repeat;background-position:center top}.es-footer-body{background-color:#fff}.es-footer-body ol li,.es-footer-body p,.es-footer-body ul li{color:#2d3142;font-size:14px}.es-footer-body a{color:#2d3142;font-size:14px}.es-infoblock,.es-infoblock ol li,.es-infoblock p,.es-infoblock ul li{line-height:120%;font-size:12px;color:#ccc}.es-infoblock a{font-size:12px;color:#ccc}h1{font-size:48px;font-style:normal;font-weight:700;color:#2d3142}h2{font-size:36px;font-style:normal;font-weight:700;color:#2d3142}h3{font-size:28px;font-style:normal;font-weight:700;color:#2d3142}.es-content-body h1 a,.es-footer-body h1 a,.es-header-body h1 a{font-size:48px}.es-content-body h2 a,.es-footer-body h2 a,.es-header-body h2 a{font-size:36px}.es-content-body h3 a,.es-footer-body h3 a,.es-header-body h3 a{font-size:28px}a.es-button,button.es-button{padding:15px 20px 15px 20px;display:inline-block;background:#4114f7;border-radius:30px;font-size:22px;font-family:Imprima,Arial,sans-serif;font-weight:700;font-style:normal;line-height:120%;color:#fff;text-decoration:none;width:auto;text-align:center;mso-padding-alt:0;mso-border-alt:10px solid #4114f7}.es-button-border{border-style:solid solid solid solid;border-color:#2cb543 #2cb543 #2cb543 #2cb543;background:#4114f7;border-width:0;display:inline-block;border-radius:30px;width:auto}.msohide{mso-hide:all}@media only screen and (max-width:600px){a,ol li,p,ul li{line-height:150%!important}h1,h1 a,h2,h2 a,h3,h3 a{line-height:120%}h1{font-size:30px!important;text-align:left}h2{font-size:24px!important;text-align:left}h3{font-size:20px!important;text-align:left}.es-content-body h1 a,.es-footer-body h1 a,.es-header-body h1 a{font-size:30px!important;text-align:left}.es-content-body h2 a,.es-footer-body h2 a,.es-header-body h2 a{font-size:24px!important;text-align:left}.es-content-body h3 a,.es-footer-body h3 a,.es-header-body h3 a{font-size:20px!important;text-align:left}.es-menu td a{font-size:14px!important}.es-header-body a,.es-header-body ol li,.es-header-body p,.es-header-body ul li{font-size:14px!important}.es-content-body a,.es-content-body ol li,.es-content-body p,.es-content-body ul li{font-size:14px!important}.es-footer-body a,.es-footer-body ol li,.es-footer-body p,.es-footer-body ul li{font-size:14px!important}.es-infoblock a,.es-infoblock ol li,.es-infoblock p,.es-infoblock ul li{font-size:12px!important}[class=gmail-fix]{display:none!important}.es-m-txt-c,.es-m-txt-c h1,.es-m-txt-c h2,.es-m-txt-c h3{text-align:center!important}.es-m-txt-r,.es-m-txt-r h1,.es-m-txt-r h2,.es-m-txt-r h3{text-align:right!important}.es-m-txt-l,.es-m-txt-l h1,.es-m-txt-l h2,.es-m-txt-l h3{text-align:left!important}.es-m-txt-c img,.es-m-txt-l img,.es-m-txt-r img{display:inline!important}.es-button-border{display:block!important}a.es-button,button.es-button{font-size:18px!important;display:block!important;border-right-width:0!important;border-left-width:0!important;border-top-width:15px!important;border-bottom-width:15px!important}.es-adaptive table,.es-left,.es-right{width:100%!important}.es-content,.es-content table,.es-footer,.es-footer table,.es-header,.es-header table{width:100%!important;max-width:600px!important}.es-adapt-td{display:block!important;width:100%!important}.adapt-img{width:100%!important;height:auto!important}.es-m-p0{padding:0!important}.es-m-p0r{padding-right:0!important}.es-m-p0l{padding-left:0!important}.es-m-p0t{padding-top:0!important}.es-m-p0b{padding-bottom:0!important}.es-m-p20b{padding-bottom:20px!important}.es-hidden,.es-mobile-hidden{display:none!important}table.es-desk-hidden,td.es-desk-hidden,tr.es-desk-hidden{width:auto!important;overflow:visible!important;float:none!important;max-height:inherit!important;line-height:inherit!important}tr.es-desk-hidden{display:table-row!important}table.es-desk-hidden{display:table!important}td.es-desk-menu-hidden{display:table-cell!important}.es-menu td{width:1%!important}.esd-block-html table,table.es-table-not-adapt{width:auto!important}table.es-social{display:inline-block!important}table.es-social td{display:inline-block!important}.es-desk-hidden{display:table-row!important;width:auto!important;overflow:visible!important;max-height:inherit!important}}body,html{font-family:arial,""helvetica neue"",helvetica,sans-serif}.es-p-default{padding-top:20px;padding-right:40px;padding-bottom:0;padding-left:40px}.es-p-all-default{padding:0}</style><div class=es-wrapper-color dir=ltr><table cellpadding=0 cellspacing=0 width=100% class=es-wrapper><tr><td class=esd-email-paddings valign=top><table cellpadding=0 cellspacing=0 class=es-content align=center><tr><td class=esd-stripe align=center><table cellpadding=0 cellspacing=0 width=600 bgcolor=#efefef style=""border-radius:20px 20px 0 0;margin-top:40px""align=center class=es-content-body><tr><td class=""es-p40l es-p40r esd-structure es-p40t""align=left><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-container-frame align=center style=""padding:40px 40px 0""valign=top width=520><table cellpadding=0 cellspacing=0 width=100%><tr><td class=""es-m-txt-c esd-block-image""align=left style=font-size:0><img alt=""Confirm email""src=https://tlr.stripocdn.email/content/guids/CABINET_ee77850a5a9f3068d9355050e69c76d26d58c3ea2927fa145f0d7a894e624758/images/group_4076323.png style=display:block;border-radius:100px title=""Confirm email""width=100></table></table><tr><td class=""es-p40l es-p40r esd-structure es-p20t""align=left style=padding:40px><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-container-frame align=center valign=top width=520><table cellpadding=0 cellspacing=0 width=100% bgcolor=#fafafa style=background-color:#fafafa;border-radius:10px;border-collapse:separate><tr><td class=""esd-block-text es-p20""align=left style=padding:20px><h3>Hello, " + @vendor.User.FullName + @"</h3><p><br><p>You're receiving this message because you recently signed up for a vendor account.<br><br>Your vendor request has been accepted by our team.</table></table></table></table><table cellpadding=0 cellspacing=0 class=es-content align=center><tr><td class=esd-stripe align=center><table cellpadding=0 cellspacing=0 width=600 bgcolor=#efefef style=""margin-bottom:40px;border-radius:0 0 20px 20px""align=center class=es-content-body><tr><td class=""es-p40l es-p40r esd-structure""align=left style=""padding:0 40px 40px""><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-container-frame align=center valign=top width=520><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-block-text align=left><p style=margin-top:20px>Thanks,<br><br>Maxmod Team</table></table></table></table></table></div>";
        mailMessage.To.Add(vendor.User.Email!);
        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task RejectVendor(Vendor vendor)
    {
        SmtpClient smtpClient = new SmtpClient(_configuration["EmailSettings:Host"], int.Parse(_configuration["EmailSettings:Port"]))
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_configuration["EmailSettings:Email"], _configuration["EmailSettings:Password"])
        };
        MailMessage mailMessage = new MailMessage()
        {
            Subject = "Maxmod Vendor Request",
            From = new MailAddress(_configuration["EmailSettings:Email"]),
            IsBodyHtml = true,
        };

        await _vendorRepository.DeleteAsync(vendor.Id);

        mailMessage.Body = @"<!doctypehtml><html dir=ltr xmlns=http://www.w3.org/1999/xhtml xmlns:o=urn:schemas-microsoft-com:office:office><meta charset=UTF-8><meta content=""width=device-width,initial-scale=1""name=viewport><meta name=x-apple-disable-message-reformatting><meta content=""IE=edge""http-equiv=X-UA-Compatible><meta content=""telephone=no""name=format-detection><title></title><style>@import url(https://fonts.googleapis.com/css2?family=Imprima&display=swap);html *{font-family:Imprima,sans-serif!important}#outlook a{padding:0}.es-button{mso-style-priority:100!important;text-decoration:none!important}a[x-apple-data-detectors]{color:inherit!important;text-decoration:none!important;font-size:inherit!important;font-family:inherit!important;font-weight:inherit!important;line-height:inherit!important}.es-desk-hidden{display:none;float:left;overflow:hidden;width:0;max-height:0;line-height:0;mso-hide:all}body{width:100%;font-family:Imprima,Arial,sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table{mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0}.es-wrapper,body,table td{padding:0;margin:0}.es-content,.es-footer,.es-header{table-layout:fixed!important;width:100%}img{display:block;border:0;outline:0;text-decoration:none;-ms-interpolation-mode:bicubic}hr,p{margin:0}h1,h2,h3,h4,h5{margin:0;line-height:120%;mso-line-height-rule:exactly;font-family:Imprima,Arial,sans-serif}a,ol li,p,ul li{-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly}.es-left{float:left}.es-right{float:right}.es-p5{padding:5px}.es-p5t{padding-top:5px}.es-p5b{padding-bottom:5px}.es-p5l{padding-left:5px}.es-p5r{padding-right:5px}.es-p10{padding:10px}.es-p10t{padding-top:10px}.es-p10b{padding-bottom:10px}.es-p10l{padding-left:10px}.es-p10r{padding-right:10px}.es-p15{padding:15px}.es-p15t{padding-top:15px}.es-p15b{padding-bottom:15px}.es-p15l{padding-left:15px}.es-p15r{padding-right:15px}.es-p20{padding:20px}.es-p20t{padding-top:20px}.es-p20b{padding-bottom:20px}.es-p20l{padding-left:20px}.es-p20r{padding-right:20px}.es-p25{padding:25px}.es-p25t{padding-top:25px}.es-p25b{padding-bottom:25px}.es-p25l{padding-left:25px}.es-p25r{padding-right:25px}.es-p30{padding:30px}.es-p30t{padding-top:30px}.es-p30b{padding-bottom:30px}.es-p30l{padding-left:30px}.es-p30r{padding-right:30px}.es-p35{padding:35px}.es-p35t{padding-top:35px}.es-p35b{padding-bottom:35px}.es-p35l{padding-left:35px}.es-p35r{padding-right:35px}.es-p40{padding:40px}.es-p40t{padding-top:40px}.es-p40b{padding-bottom:40px}.es-p40l{padding-left:40px}.es-p40r{padding-right:40px}.es-menu td{border:0}.es-menu td a img{display:inline-block!important;vertical-align:middle}s{text-decoration:line-through}ol li,p,ul li{font-family:Imprima,Arial,sans-serif;line-height:150%}ol li,ul li{margin-bottom:15px;margin-left:0}a{text-decoration:underline}.es-menu td a{text-decoration:none;display:block;font-family:Imprima,Arial,sans-serif}.es-wrapper{width:100%;height:100%;background-repeat:repeat;background-position:center top}.es-wrapper,.es-wrapper-color{background-color:#fff}.es-header{background-color:transparent;background-repeat:repeat;background-position:center top}.es-header-body{background-color:#efefef}.es-header-body ol li,.es-header-body p,.es-header-body ul li{color:#2d3142;font-size:14px}.es-header-body a{color:#2d3142;font-size:14px}.es-content-body{background-color:#efefef}.es-content-body ol li,.es-content-body p,.es-content-body ul li{color:#2d3142;font-size:18px}.es-content-body a{color:#2d3142;font-size:18px}.es-footer{background-color:transparent;background-repeat:repeat;background-position:center top}.es-footer-body{background-color:#fff}.es-footer-body ol li,.es-footer-body p,.es-footer-body ul li{color:#2d3142;font-size:14px}.es-footer-body a{color:#2d3142;font-size:14px}.es-infoblock,.es-infoblock ol li,.es-infoblock p,.es-infoblock ul li{line-height:120%;font-size:12px;color:#ccc}.es-infoblock a{font-size:12px;color:#ccc}h1{font-size:48px;font-style:normal;font-weight:700;color:#2d3142}h2{font-size:36px;font-style:normal;font-weight:700;color:#2d3142}h3{font-size:28px;font-style:normal;font-weight:700;color:#2d3142}.es-content-body h1 a,.es-footer-body h1 a,.es-header-body h1 a{font-size:48px}.es-content-body h2 a,.es-footer-body h2 a,.es-header-body h2 a{font-size:36px}.es-content-body h3 a,.es-footer-body h3 a,.es-header-body h3 a{font-size:28px}a.es-button,button.es-button{padding:15px 20px 15px 20px;display:inline-block;background:#4114f7;border-radius:30px;font-size:22px;font-family:Imprima,Arial,sans-serif;font-weight:700;font-style:normal;line-height:120%;color:#fff;text-decoration:none;width:auto;text-align:center;mso-padding-alt:0;mso-border-alt:10px solid #4114f7}.es-button-border{border-style:solid solid solid solid;border-color:#2cb543 #2cb543 #2cb543 #2cb543;background:#4114f7;border-width:0;display:inline-block;border-radius:30px;width:auto}.msohide{mso-hide:all}@media only screen and (max-width:600px){a,ol li,p,ul li{line-height:150%!important}h1,h1 a,h2,h2 a,h3,h3 a{line-height:120%}h1{font-size:30px!important;text-align:left}h2{font-size:24px!important;text-align:left}h3{font-size:20px!important;text-align:left}.es-content-body h1 a,.es-footer-body h1 a,.es-header-body h1 a{font-size:30px!important;text-align:left}.es-content-body h2 a,.es-footer-body h2 a,.es-header-body h2 a{font-size:24px!important;text-align:left}.es-content-body h3 a,.es-footer-body h3 a,.es-header-body h3 a{font-size:20px!important;text-align:left}.es-menu td a{font-size:14px!important}.es-header-body a,.es-header-body ol li,.es-header-body p,.es-header-body ul li{font-size:14px!important}.es-content-body a,.es-content-body ol li,.es-content-body p,.es-content-body ul li{font-size:14px!important}.es-footer-body a,.es-footer-body ol li,.es-footer-body p,.es-footer-body ul li{font-size:14px!important}.es-infoblock a,.es-infoblock ol li,.es-infoblock p,.es-infoblock ul li{font-size:12px!important}[class=gmail-fix]{display:none!important}.es-m-txt-c,.es-m-txt-c h1,.es-m-txt-c h2,.es-m-txt-c h3{text-align:center!important}.es-m-txt-r,.es-m-txt-r h1,.es-m-txt-r h2,.es-m-txt-r h3{text-align:right!important}.es-m-txt-l,.es-m-txt-l h1,.es-m-txt-l h2,.es-m-txt-l h3{text-align:left!important}.es-m-txt-c img,.es-m-txt-l img,.es-m-txt-r img{display:inline!important}.es-button-border{display:block!important}a.es-button,button.es-button{font-size:18px!important;display:block!important;border-right-width:0!important;border-left-width:0!important;border-top-width:15px!important;border-bottom-width:15px!important}.es-adaptive table,.es-left,.es-right{width:100%!important}.es-content,.es-content table,.es-footer,.es-footer table,.es-header,.es-header table{width:100%!important;max-width:600px!important}.es-adapt-td{display:block!important;width:100%!important}.adapt-img{width:100%!important;height:auto!important}.es-m-p0{padding:0!important}.es-m-p0r{padding-right:0!important}.es-m-p0l{padding-left:0!important}.es-m-p0t{padding-top:0!important}.es-m-p0b{padding-bottom:0!important}.es-m-p20b{padding-bottom:20px!important}.es-hidden,.es-mobile-hidden{display:none!important}table.es-desk-hidden,td.es-desk-hidden,tr.es-desk-hidden{width:auto!important;overflow:visible!important;float:none!important;max-height:inherit!important;line-height:inherit!important}tr.es-desk-hidden{display:table-row!important}table.es-desk-hidden{display:table!important}td.es-desk-menu-hidden{display:table-cell!important}.es-menu td{width:1%!important}.esd-block-html table,table.es-table-not-adapt{width:auto!important}table.es-social{display:inline-block!important}table.es-social td{display:inline-block!important}.es-desk-hidden{display:table-row!important;width:auto!important;overflow:visible!important;max-height:inherit!important}}body,html{font-family:arial,""helvetica neue"",helvetica,sans-serif}.es-p-default{padding-top:20px;padding-right:40px;padding-bottom:0;padding-left:40px}.es-p-all-default{padding:0}</style><div class=es-wrapper-color dir=ltr><table cellpadding=0 cellspacing=0 width=100% class=es-wrapper><tr><td class=esd-email-paddings valign=top><table cellpadding=0 cellspacing=0 class=es-content align=center><tr><td class=esd-stripe align=center><table cellpadding=0 cellspacing=0 width=600 bgcolor=#efefef style=""border-radius:20px 20px 0 0;margin-top:40px""align=center class=es-content-body><tr><td class=""es-p40l es-p40r esd-structure es-p40t""align=left><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-container-frame align=center style=""padding:40px 40px 0""valign=top width=520><table cellpadding=0 cellspacing=0 width=100%><tr><td class=""es-m-txt-c esd-block-image""align=left style=font-size:0><img alt=""Confirm email""src=https://tlr.stripocdn.email/content/guids/CABINET_ee77850a5a9f3068d9355050e69c76d26d58c3ea2927fa145f0d7a894e624758/images/group_4076323.png style=display:block;border-radius:100px title=""Confirm email""width=100></table></table><tr><td class=""es-p40l es-p40r esd-structure es-p20t""align=left style=padding:40px><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-container-frame align=center valign=top width=520><table cellpadding=0 cellspacing=0 width=100% bgcolor=#fafafa style=background-color:#fafafa;border-radius:10px;border-collapse:separate><tr><td class=""esd-block-text es-p20""align=left style=padding:20px><h3>Hello, " + @vendor.User.FullName + @"</h3><p><br><p>You're receiving this message because you recently signed up for a vendor account.<br><br>Your vendor request has been rejected by our team.</table></table></table></table><table cellpadding=0 cellspacing=0 class=es-content align=center><tr><td class=esd-stripe align=center><table cellpadding=0 cellspacing=0 width=600 bgcolor=#efefef style=""margin-bottom:40px;border-radius:0 0 20px 20px""align=center class=es-content-body><tr><td class=""es-p40l es-p40r esd-structure""align=left style=""padding:0 40px 40px""><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-container-frame align=center valign=top width=520><table cellpadding=0 cellspacing=0 width=100%><tr><td class=esd-block-text align=left><p style=margin-top:20px>Thanks,<br><br>Maxmod Team</table></table></table></table></table></div>";
        mailMessage.To.Add(vendor.User.Email!);
        await smtpClient.SendMailAsync(mailMessage);
    }
}
