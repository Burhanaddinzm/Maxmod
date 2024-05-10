using Maxmod.Data.Contexts;
using Maxmod.Enums;
using Maxmod.Models;
using Microsoft.AspNetCore.Identity;

namespace Maxmod.Data;

public static class SeedData
{
    public static void SeedSettings(this IServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!dbContext.Settings.Any())
        {
            var settings = new Settings
            {
                IntroTitle = "Intro Title",
                IntroDescription = "Intro Description",
                IntroImage = "intro_img.png",
                MarqueeText1 = "Marquee Text 1",
                MarqueeText2 = "Marquee Text 2",
                PromoTitle = "Promo Title",
                PromoDescription = "Promo Description",
                PromoImage = "bnr1.png",
                SmallPromoHeading1 = "Small Promo Heading 1",
                SmallPromoTitle1 = "Small Promo Title 1",
                SmallPromoText1 = "Small Promo Text 1",
                SmallPromoImage1 = "bnr2.png",
                SmallPromoHeading2 = "Small Promo Heading 2",
                SmallPromoTitle2 = "Small Promo Title 2",
                SmallPromoText2 = "Small Promo Text 2",
                SmallPromoImage2 = "b2.png",
                SmallPromoHeading3 = "Small Promo Heading 3",
                SmallPromoTitle3 = "Small Promo Title 3",
                SmallPromoText3 = "Small Promo Text 3",
                SmallPromoImage3 = "b3.png",
                BigPromoHeading = "Big Promo Heading",
                BigPromoTitle = "Big Promo Title",
                BigPromoText = "Big Promo Text",
                BigPromoImage = "bnr3.png",
                Address = "4800 San Mateo Ln NE",
                Phone = "0123-456-789",
                Email = "demo@demo.com",
                ContactText = "Contact Text",
                AboutTitle = "About Title",
                AboutDescription = "About Description",
                AboutBoxHeading = "About Box Heading",
                AboutBoxTitle = "About Box Title",
                AboutBoxText = "About Box Text",
                AboutBoxImage = "fruit-snacks-at-picnic.png"
            };

            dbContext.Settings.Add(settings);
            dbContext.SaveChanges();
        }
    }

    public static async Task SeedAdmin(this IServiceScope scope)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string Id = Guid.NewGuid().ToString();
        string FullName = "Admin Admin";
        string Email = "admin@admin.com";
        string Password = "Admin123";

        if (await userManager.FindByEmailAsync(Email) == null)
        {
            var user = new AppUser
            {
                Id = Id,
                UserName = FullName.Trim().Replace(' ', '_'),
                FullName = FullName,
                Email = Email
            };
            var result = await userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                foreach (var role in Enum.GetValues(typeof(Roles)))
                {
                    if (!await roleManager.RoleExistsAsync(role.ToString()!))
                        await roleManager.CreateAsync(new IdentityRole(role.ToString()!));
                }

                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }
    }
}
