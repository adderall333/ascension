using System;
using System.Collections.Generic;
using System.Linq;
using Ascension.Data;
using Ascension.Models;
using Microsoft.EntityFrameworkCore;

namespace Ascension.Services
{
    public static class Sorter
    {
        public static IEnumerable<Product> Sort(this IEnumerable<Product> products, string sortOption)
        {
            return sortOption == null ? products : sortOption switch
            {
                "cheapFirst" => products.OrderBy(p => p.Cost),
                "expensiveFirst" => products.OrderByDescending(p => p.Cost),
                "alphabet" => products.OrderBy(p => p.Name),
                _ => throw new ArgumentException("There is no such sort option")
            };
        }
        
        public static IEnumerable<Product> FilterProducts(string categoryName, string optionIds)
        {
            var context = new ApplicationContext();
            
            var ids = optionIds
                .Select(e => (int)char.GetNumericValue(e))
                .ToList();
            
            var specifications = context
                .Specification
                .Where(s => s.Category.Name == categoryName)
                .Include(s => s.SpecificationOptions)
                .ToList();

            var checkedOptions = context
                .SpecificationOption
                .Where(sOp => ids.Contains(sOp.Id))
                .ToList();

            var options = specifications
                .Where(s => !s
                    .SpecificationOptions
                    .Any(sOp => ids.Contains(sOp.Id)))
                .SelectMany(s => s.SpecificationOptions)
                .Concat(checkedOptions);
            
            foreach (var product in context
                .Product
                .Include(p => p.Images)
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