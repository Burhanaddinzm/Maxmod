using Maxmod.Models.Common;

namespace Maxmod.Models;

public class Settings : BaseAuditableEntity
{
    public string IntroTitle { get; set; } = null!;
    public string IntroDescription { get; set; } = null!;
    public string IntroImage { get; set; } = null!;
    public string MarqueeText1 { get; set; } = null!;
    public string MarqueeText2 { get; set; } = null!;
    public string PromoTitle { get; set; } = null!;
    public string PromoDescription { get; set; } = null!;
    public string PromoImage { get; set; } = null!;
    public string SmallPromoHeading1 { get; set; } = null!;
    public string SmallPromoTitle1 { get; set; } = null!;
    public string SmallPromoText1 { get; set; } = null!;
    public string SmallPromoImage1 { get; set; } = null!;
    public string SmallPromoHeading2 { get; set; } = null!;
    public string SmallPromoTitle2 { get; set; } = null!;
    public string SmallPromoText2 { get; set; } = null!;
    public string SmallPromoImage2 { get; set; } = null!;
    public string SmallPromoHeading3 { get; set; } = null!;
    public string SmallPromoTitle3 { get; set; } = null!;
    public string SmallPromoText3 { get; set; } = null!;
    public string SmallPromoImage3 { get; set; } = null!;
    public string BigPromoHeading { get; set; } = null!;
    public string BigPromoTitle { get; set; } = null!;
    public string BigPromoText { get; set; } = null!;
    public string BigPromoImage { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ContactText { get; set; } = null!;
    public string AboutTitle { get; set; } = null!;
    public string AboutDescription { get; set; } = null!;
    public string AboutBoxHeading { get; set; } = null!;
    public string AboutBoxTitle { get; set; } = null!;
    public string AboutBoxText { get; set; } = null!;
    public string AboutBoxImage { get; set; } = null!;
}
