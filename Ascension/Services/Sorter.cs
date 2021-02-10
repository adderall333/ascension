using System;
using System.Collections.Generic;
using System.Linq;
using Ascension.Models;

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
    }
}