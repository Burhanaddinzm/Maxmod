using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Maxmod.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IWeightService _weightService;

    public ProductController(IProductService productService, ICategoryService categoryService, IWeightService weightService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _weightService = weightService;
    }

    public async Task<IActionResult> Index(string? category, string? orderBy, string? orderByDesc, string? searchString, string? inStock, string? weights, int page = 1)
    {
        // Parameter validation and defaults
        if (orderByDesc == null && orderBy == null)
        {
            orderByDesc = "CreatedAt";
        }
        page = page < 1 ? 1 : page;

        // Fetch categories and weights
        var categories = await _categoryService.GetAllCategoriesAsync(null, null, null, null, "Parent", "SubCategories");
        var weightList = await _weightService.GetAllWeightsAsync(x => x.Name != "Default");

        List<string>? weightFilters = weights?.Split(',').ToList();

        // Fetch products
        var products = await _productService.FetchClientProductsAsync(category, orderBy, orderByDesc, searchString, weightFilters);

        // Apply in-stock filter
        products = _productService.ApplyInStockFilter(products, inStock);

        // Pagination logic
        const int pageSize = 6;
        int itemCount = products.Count();
        if (itemCount == 0)
        {
            TempData["Error"] = "Products not found!";
            return RedirectToAction("Index", "Error");
        }

        var pager = new PagerVM(itemCount, page, pageSize);
        var paginatedProducts = _productService.PaginateProduct(pager, products);

        // Assign data to ViewBag
        ViewBag.Pager = pager;
        ViewBag.Category = category;
        ViewBag.OrderBy = orderBy;
        ViewBag.OrderByDesc = orderByDesc;
        ViewBag.SerachString = searchString;
        ViewBag.Categories = categories;
        ViewBag.Weights = weightList;
        ViewBag.InStock = inStock;
        ViewBag.SelectedWeights = weightFilters;

        return View(paginatedProducts);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var (exists, product) = await _productService.CheckExistanceAsync(id);
        if (!exists) return RedirectToAction("Index", "Error");
        if (!product!.ProductWeights.Any(x => x.Stock > 0))
        {
            TempData["Error"] = "Product not found!";
            return RedirectToAction("Index", "Error");
        }
        return View(product);
    }
}
