﻿using Maxmod.Models;
using Maxmod.Models.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Maxmod.Data.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    private readonly IHttpContextAccessor _accessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor accessor) : base(options)
    {
        _accessor = accessor;
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductWeight> ProductWeights => Set<ProductWeight>();
    public DbSet<Weight> Weights => Set<Weight>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Settings> Settings => Set<Settings>();
    public DbSet<Cart> Cart => Set<Cart>();
    public DbSet<Order> Orders => Set<Order>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseAuditableEntity>();
        var currentUserName = _accessor.HttpContext?.User.Identity?.Name ?? "Anonymous";
        var currentIpAddress = _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserName;
                    entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                    entry.Entity.IP = currentIpAddress;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedBy = currentUserName;
                    entry.Entity.ModifiedAt = DateTime.UtcNow.AddHours(4);
                    entry.Entity.IP = currentIpAddress;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ProductImage>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ProductWeight>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Vendor>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Weight>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Cart>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(x => !x.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}
