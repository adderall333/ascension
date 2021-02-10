using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ascension.Data;
using Ascension.Models;
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
                .Include(c => c.Specifications)
                .ThenInclude(s => s.SpecificationOptions)
                .AsSplitQuery()
                .ToListAsync();
            return View(models.First());
        }

        public async Task<PartialViewResult> GetProducts(string sortOption, string category, string ids)
        {
            ViewData["sortOption"] = sortOption;
            if (ids == null)
                return PartialView("ProductsPartial", await _context
                    .Product
                    .Include(p => p.Category)
                    .Where(p => p.Category.Name == category)
                    .ToListAsync());
            var products = FilterProducts(category, ids);
            return PartialView("ProductsPartial", products.ToList());
        }
        
        private IEnumerable<Product> FilterProducts(string categoryName, string optionIds)
        {
            var ids = optionIds
                .Select(e => (int)char.GetNumericValue(e))
                .ToList();
            
            var specifications = _context
                .Specification
                .Where(s => s.Category.Name == categoryName)
                .Include(s => s.SpecificationOptions)
                .ToList();

            var checkedOptions = _context
                .SpecificationOption
                .Where(sOp => ids.Contains(sOp.Id))
                .ToList();

            var options = specifications
                .Where(s => !s
                    .SpecificationOptions
                    .Any(sOp => ids.Contains(sOp.Id)))
                .SelectMany(s => s.SpecificationOptions)
                .Concat(checkedOptions);
            
            foreach (var product in _context
                .Product
                .Include(p => p.SpecificationOptions)
                .Include(p => p.Category)
                .Where(p => p.Category.Name == categoryName)
                .Where(product => product
                    .SpecificationOptions
                    .All(sOp => options.Contains(sOp))))
            {
                yield return product;
            }
        }
    }
}