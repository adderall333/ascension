﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;

namespace Models
{
    public class SpecificationOption : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
        
        [NotAdministered]
        public int SpecificationId { get; set; }
        
        [ManyToOne]
        public Specification Specification { get; set; }
        
        [ManyToMany]
        public IEnumerable<Product> Products { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name}";
        }
        
        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            return context
                .SpecificationOption
                .Include(specificationOption => specificationOption.Specification)
                .Include(specificationOption => specificationOption.Products)
                .First(specificationOption => specificationOption.Id == id);
        }

        public SpecificationOption(string name, int specification, List<int> products)
        {
            var context = new ApplicationContext();
            Name = name;
            Specification = context.Specification.First(s => s.Id == specification);
            Products = context.Product.Where(product => products.Contains(product.Id));
        }

        public SpecificationOption()
        {
        }
    }
}