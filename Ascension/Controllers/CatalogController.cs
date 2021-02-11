using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ascension.Data;
using Ascension.Models;
using Ascension.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ascension.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationContext _context;

        public CatalogController(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<ViewResult> Index()
        {
            return View(await _context
                .SuperCategory
                .Include(sc => sc.Categories)
                .ToListAsync());
        }

        public async Task<ViewResult> Category(string name)
        {
            var models = await _context
                .Category
                .Where(c => c.Name == name)
                .Include(c => c.Products)
                .ThenInclude(p => p.Images)
                .Include(c => c.Specifications)
                .ThenInclude(s => s.SpecificationOptions)
                .AsSplitQuery()
                .FirstAsync();
            return View(models);
        }

        public async Task<PartialViewResult> GetProducts(string sortOption, string category, string ids)
        {
            ViewData["sortOption"] = sortOption;
            if (ids == null)
                return PartialView("ProductsPartial", await _context
                    .Product
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .Where(p => p.Category.Name == category)
                    .AsSplitQuery()
                    .ToListAsync());
            var products = Sorter.FilterProducts(category, ids);
            return PartialView("ProductsPartial", products.ToList());
        }

        public async Task<ViewResult> Product(int id)
        {
            return View(await _context
                .Product
                .Where(p => p.Id == id)
                .Include(p => p.Images)
                .Include(p => p.SpecificationOptions)
                .ThenInclude(sOp => sOp.Specification)
                .AsSplitQuery()
                .FirstAsync());
        }
    }
}