using Maxmod.Areas.Admin.ViewModels.Category;

namespace Maxmod.Areas.Admin.ViewModels.Settings;

public class UpdateSettingsVM
{
    public int Id { get; set; }
    public string IntroTitle { get; set; } = null!;
    public string IntroDescription { get; set; } = null!;
    public IFormFile? IntroImage { get; set; }
    public string MarqueeText1 { get; set; } = null!;
    public string MarqueeText2 { get; set; } = null!;
    public string PromoTitle { get; set; } = null!;
    public string PromoDescription { get; set; } = null!;
    public IFormFile? PromoImage { get; set; }
    public string SmallPromoHeading1 { get; set; } = null!;
    public string SmallPromoTitle1 { get; set; } = null!;
    public string SmallPromoText1 { get; set; } = null!;
    public IFormFile? SmallPromoImage1 { get; set; }
    public string SmallPromoHeading2 { get; set; } = null!;
    public string SmallPromoTitle2 { get; set; } = null!;
    public string SmallPromoText2 { get; set; } = null!;
    public IFormFile? SmallPromoImage2 { get; set; }
    public string SmallPromoHeading3 { get; set; } = null!;
    public string SmallPromoTitle3 { get; set; } = null!;
    public string SmallPromoText3 { get; set; } = null!;
    public IFormFile? SmallPromoImage3 { get; set; }
    public string BigPromoHeading { get; set; } = null!;
    public string BigPromoTitle { get; set; } = null!;
    public string BigPromoText { get; set; } = null!;
    public IFormFile? BigPromoImage { get; set; }
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ContactText { get; set; } = null!;
    public string AboutTitle { get; set; } = null!;
    public string AboutDescription { get; set; } = null!;
    public string AboutBoxHeading { get; set; } = null!;
    public string AboutBoxTitle { get; set; } = null!;
    public string AboutBoxText { get; set; } = null!;
    public IFormFile? AboutBoxImage { get; set; }

    public static implicit operator UpdateSettingsVM(Models.Settings settings)
    {
        return new UpdateSettingsVM
        {
            Id = settings.Id,
            IntroTitle = settings.IntroTitle,
            IntroDescription = settings.IntroDescription,
            MarqueeText1 = settings.MarqueeText1,
            MarqueeText2 = settings.MarqueeText2,
            PromoTitle = settings.PromoTitle,
            PromoDescription = settings.PromoDescription,
            SmallPromoHeading1 = settings.SmallPromoHeading1,
            SmallPromoTitle1 = settings.SmallPromoTitle1,
            SmallPromoText1 = settings.SmallPromoText1,
            SmallPromoHeading2 = settings.SmallPromoHeading2,
            SmallPromoTitle2 = settings.SmallPromoTitle2,
            SmallPromoText2 = settings.SmallPromoText2,
            SmallPromoHeading3 = settings.SmallPromoHeading3,
            SmallPromoTitle3 = settings.SmallPromoTitle3,
            SmallPromoText3 = settings.SmallPromoText3,
            BigPromoHeading = settings.BigPromoHeading,
            BigPromoTitle = settings.BigPromoTitle,
            BigPromoText = settings.BigPromoText,
            Address = settings.Address,
            Phone = settings.Phone,
            Email = settings.Email,
            ContactText = settings.ContactText,
            AboutTitle = settings.AboutTitle,
            AboutDescription = settings.AboutDescription,
            AboutBoxHeading = settings.AboutBoxHeading,
            AboutBoxTitle = settings.AboutBoxTitle,
            AboutBoxText = settings.AboutBoxText,
        };
    }
}
