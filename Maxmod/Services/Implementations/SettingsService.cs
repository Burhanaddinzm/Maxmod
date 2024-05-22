using Maxmod.Areas.Admin.ViewModels.Category;
using Maxmod.Areas.Admin.ViewModels.Settings;
using Maxmod.Models;
using Maxmod.Repositories.Implementations;
using Maxmod.Repositories.Interfaces;
using Maxmod.Services.Interfaces;
using static Maxmod.Extensions.FileExtension;

namespace Maxmod.Services.Implementations;

public class SettingsService : ISettingsService
{
    private readonly ISettingsRepository _settingsRepository;
    private readonly IWebHostEnvironment _env;

    public SettingsService(ISettingsRepository settingsRepository, IWebHostEnvironment env)
    {
        _settingsRepository = settingsRepository;
        _env = env;
    }

    public async Task<Settings> GetSettingsAsync()
    {
        return await _settingsRepository.GetAsync(null);
    }

    public async Task<FileValidationResult?> UpdateSettingsAsync(UpdateSettingsVM updateSettingsVM)
    {
        var settings = await GetSettingsAsync();

        settings.IntroTitle = updateSettingsVM.IntroTitle;
        settings.IntroDescription = updateSettingsVM.IntroDescription;

        if (updateSettingsVM.IntroImage != null)
        {
            var validationResult = updateSettingsVM.IntroImage.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateSettingsVM.IntroImage.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "misc");
            if (settings.IntroImage != "intro_img.png")
                updateSettingsVM.IntroImage.DeleteFile(_env.WebRootPath, "client", "assets", "images", "misc", settings.IntroImage);
            settings.IntroImage = filename;
        }

        settings.MarqueeText1 = updateSettingsVM.MarqueeText1;
        settings.MarqueeText2 = updateSettingsVM.MarqueeText2;
        settings.PromoTitle = updateSettingsVM.PromoTitle;
        settings.PromoDescription = updateSettingsVM.PromoDescription;

        if (updateSettingsVM.PromoImage != null)
        {
            var validationResult = updateSettingsVM.PromoImage.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateSettingsVM.PromoImage.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "misc");
            if (settings.PromoImage != "bnr1.png")
                updateSettingsVM.PromoImage.DeleteFile(_env.WebRootPath, "client", "assets", "images", "misc", settings.PromoImage);
            settings.PromoImage = filename;
        }

        settings.SmallPromoHeading1 = updateSettingsVM.SmallPromoHeading1;
        settings.SmallPromoHeading2 = updateSettingsVM.SmallPromoHeading2;
        settings.SmallPromoHeading3 = updateSettingsVM.SmallPromoHeading3;
        settings.SmallPromoTitle1 = updateSettingsVM.SmallPromoTitle1;
        settings.SmallPromoTitle2 = updateSettingsVM.SmallPromoTitle2;
        settings.SmallPromoTitle3 = updateSettingsVM.SmallPromoTitle3;
        settings.SmallPromoText1 = updateSettingsVM.SmallPromoText1;
        settings.SmallPromoText2 = updateSettingsVM.SmallPromoText2;
        settings.SmallPromoText3 = updateSettingsVM.SmallPromoText3;

        if (updateSettingsVM.SmallPromoImage1 != null)
        {
            var validationResult = updateSettingsVM.SmallPromoImage1.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateSettingsVM.SmallPromoImage1.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "misc");
            if (settings.SmallPromoImage1 != "bnr2.png")
                updateSettingsVM.SmallPromoImage1.DeleteFile(_env.WebRootPath, "client", "assets", "images", "misc", settings.SmallPromoImage1);
            settings.SmallPromoImage1 = filename;
        }
        if (updateSettingsVM.SmallPromoImage2 != null)
        {
            var validationResult = updateSettingsVM.SmallPromoImage2.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateSettingsVM.SmallPromoImage2.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "misc");
            if (settings.SmallPromoImage2 != "b2.png")
                updateSettingsVM.SmallPromoImage2.DeleteFile(_env.WebRootPath, "client", "assets", "images", "misc", settings.SmallPromoImage2);
            settings.SmallPromoImage2 = filename;
        }
        if (updateSettingsVM.SmallPromoImage3 != null)
        {
            var validationResult = updateSettingsVM.SmallPromoImage3.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateSettingsVM.SmallPromoImage3.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "misc");
            if (settings.SmallPromoImage3 != "b3.png")
                updateSettingsVM.SmallPromoImage3.DeleteFile(_env.WebRootPath, "client", "assets", "images", "misc", settings.SmallPromoImage3);
            settings.SmallPromoImage3 = filename;
        }

        settings.BigPromoHeading = updateSettingsVM.BigPromoHeading;
        settings.BigPromoTitle = updateSettingsVM.BigPromoTitle;
        settings.BigPromoText = updateSettingsVM.BigPromoText;

        if (updateSettingsVM.BigPromoImage != null)
        {
            var validationResult = updateSettingsVM.BigPromoImage.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateSettingsVM.BigPromoImage.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "misc");
            if (settings.BigPromoImage != "bnr3.png")
                updateSettingsVM.BigPromoImage.DeleteFile(_env.WebRootPath, "client", "assets", "images", "misc", settings.BigPromoImage);
            settings.BigPromoImage = filename;
        }

        settings.Address = updateSettingsVM.Address;
        settings.Phone = updateSettingsVM.Phone;
        settings.Email = updateSettingsVM.Email;
        settings.ContactText = updateSettingsVM.ContactText;

        settings.AboutTitle = updateSettingsVM.AboutTitle;
        settings.AboutDescription = updateSettingsVM.AboutDescription;
        settings.AboutBoxHeading = updateSettingsVM.AboutBoxHeading;
        settings.AboutBoxTitle = updateSettingsVM.AboutBoxTitle;
        settings.AboutBoxText = updateSettingsVM.AboutBoxText;

        if (updateSettingsVM.AboutBoxImage != null)
        {
            var validationResult = updateSettingsVM.AboutBoxImage.ValidateFile();
            if (!validationResult.IsValid) return validationResult;

            var filename = await updateSettingsVM.AboutBoxImage.SaveFileAsync(_env.WebRootPath, "client", "assets", "images", "misc");
            if (settings.AboutBoxImage != "fruit-snacks-at-picnic.png")
                updateSettingsVM.AboutBoxImage.DeleteFile(_env.WebRootPath, "client", "assets", "images", "misc", settings.AboutBoxImage);
            settings.AboutBoxImage = filename;
        }

        await _settingsRepository.UpdateAsync(settings);
        return null;
    }
}