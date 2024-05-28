using Maxmod.Data.Contexts;
using Maxmod.Models;
using Maxmod.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Maxmod.Repositories.Implementations;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Product>> GetAllAsync(
         Expression<Func<Product, bool>>? where = null,
         string? order = null,
         string? orderByDesc = null,
         int? take = null,
         params string[] includes)
    {
        IQueryable<Product> query = _context.Set<Product>().AsQueryable();

        if (includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (where != null)
        {
            query = query.Where(where);
        }

        if (order == "Price")
        {
            query = ApplyProductPriceOrderBy(query, false);
        }
        else if (orderByDesc == "Price")
        {
            query = ApplyProductPriceOrderBy(query, true);
        }
        else
        {
            if (!string.IsNullOrEmpty(order))
            {
                query = ApplyOrderBy(query, order, false);
            }

            if (!string.IsNullOrEmpty(orderByDesc))
            {
                query = ApplyOrderBy(query, orderByDesc, true);
            }
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }

    private IQueryable<Product> ApplyProductPriceOrderBy(IQueryable<Product> query, bool descending)
    {
        if (descending)
        {
            return query
                .OrderByDescending(p => p.ProductWeights
                    .Where(pw => pw.Stock > 0)
                    .Min(pw => pw.DiscountPrice != 0 ? pw.DiscountPrice : pw.Price));
        }
        else
        {
            return query
                .OrderBy(p => p.ProductWeights
                    .Where(pw => pw.Stock > 0)
                    .Min(pw => pw.DiscountPrice != 0 ? pw.DiscountPrice : pw.Price));
        }
    }
}
