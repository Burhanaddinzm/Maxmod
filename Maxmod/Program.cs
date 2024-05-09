using Maxmod.Data;
using Maxmod.Data.Contexts;
using Maxmod.Enums;
using Maxmod.Models;
using Maxmod.Services;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddData(builder.Configuration);
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Seed Settings
    if (!dbContext.Settings.Any())
    {
        var settings = new Settings
        {
            IntroTitle = "Your Intro Title",
            IntroDescription = "Your Intro Description",
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

    // Seed User
    string Id = Guid.NewGuid().ToString();
    string FullName = "Admin Admin";
    string Email = "admin@admin.com";
    string Password = "Admin123";

    if (await userManager.FindByEmailAsync("admin@admin.com") == null)
    {
        var user = new AppUser
        {
            Id = Id,
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

app.Run();
