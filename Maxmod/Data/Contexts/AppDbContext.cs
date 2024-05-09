using Maxmod.Models.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace Maxmod.Data.Contexts;

public class AppDbContext : DbContext
{
    //readonly IHttpContextAccessor _accessor;
    //public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor accessor) : base(options)
    //{
    //    _accessor = accessor;
    //}

    //public DbSet<Category> Categories => Set<Category>();

    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //    var entries = ChangeTracker.Entries<BaseAuditable>();

    //    foreach (var entry in entries)
    //    {
    //        switch (entry.State)
    //        {
    //            case EntityState.Added:
    //                entry.Entity.CreatedBy = _accessor.HttpContext.User.Identity.Name ?? "newUser";
    //                entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
    //                entry.Entity.IP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
    //                break;
    //            case EntityState.Modified:
    //                entry.Entity.ModifiedBy = _accessor.HttpContext.User.Identity!.Name!;
    //                entry.Entity.ModifiedAt = DateTime.UtcNow.AddHours(4);
    //                entry.Entity.IP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    return base.SaveChangesAsync(cancellationToken);
    //}
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<BaseModel>().HasQueryFilter(x => !x.IsDeleted);
    }
}
