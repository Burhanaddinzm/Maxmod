﻿using Maxmod.Models;
using Maxmod.Services.Interfaces;
using Maxmod.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Maxmod.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? category, string? orderBy, string? orderByDesc, int page = 1)
    {
        var products = new List<Product>();
        var categories = await _categoryService.GetAllCategoriesAsync(null, null, null, null, "Parent", "SubCategories");

        if (orderByDesc == null && orderBy == null)
        {
            orderByDesc = "CreatedAt";
        }

        if (category != null)
        {
            products = await _productService.GetAllProductsAsync(
                x => x.Category.Name == category || x.Category.Parent.Name == category,
                orderBy, orderByDesc, null,
                "ProductWeights.Weight", "Vendor", "ProductImages");
        }
        else
        {
            products = await _productService.GetAllProductsAsync(
                         null, orderBy, orderByDesc, null,
                         "ProductWeights.Weight", "Vendor", "ProductImages");
        }

        const int pageSize = 6;
        if (page < 1) page = 1;

        int itemCount = products.Count();

        if (itemCount == 0)
        {
            TempData["Error"] = "Products not found!";
            return RedirectToAction("Index", "Error");
        }

        var pager = new PagerVM(itemCount, page, pageSize);

        int itemsToSkip = (page - 1) * pageSize;

        var data = products.Skip(itemsToSkip).Take(pager.PageSize).ToList();

        ViewBag.Pager = pager;
        ViewBag.Category = category;
        ViewBag.OrderBy = orderBy;
        ViewBag.OrderByDesc = orderByDesc;
        ViewBag.Categories = categories;

        return View(data);
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
