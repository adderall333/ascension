using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;
using NpgsqlTypes;

namespace Models
{
    public class Product : IModel, ICategorized
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
        
        [SimpleProperty]
        public int Cost { get; set; }
        
        [SimpleProperty]
        public string? Description { get; set; }
        
        [SimpleProperty]
        public bool IsAvailable { get; set; }
        
        [NotAdministered]
        public int CategoryId { get; set; }
        
        [ManyToOne]
        public Category Category { get; set; }
        
        [ManyToMany]
        public List<SpecificationOption> SpecificationOptions { get; set; }
        
        [OneToMany]
        public List<Image> Images { get; set; }
        
        [NotAdministered]
        public List<Purchase> Purchases { get; set; }
        
        [NotAdministered]
        public NpgsqlTsVector SearchVector { get; set; }
        
        [NotAdministered]
        public IEnumerable<Review> Reviews { get; set; }
        
        [NotAdministered]
        public ProductRating? Rating { get; set; }
        
        [NotMapped]
        [NotAdministered]
        public bool IsInCart { get; set; }
        
        [NotMapped]
        [NotAdministered]
        public string CategoryName { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name}";
        }

        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            var model = context
                .Product
                .Include(product => product.Category)
                .Include(product => product.SpecificationOptions)
                .Include(product => product.Images)
                .First(product => product.Id == id);
            model.CategoryName = model.Category.Name;
            return model;
        }

        public Product Update(string name, int cost, string description, string isAvailable, int category, List<int> specificationOptions, ApplicationContext context)
        {
            Name = name;
            Cost = cost;
            Description = description;
            IsAvailable = isAvailable == "IsAvailable";
            
            if (category > 0)
                Category = context.Category.First(c => c.Id == category);

            
            SpecificationOptions = context
                .SpecificationOption
                .Include(sOp => sOp.Products)
                .Where(sOp => sOp
                    .Products
                    .Select(p => p.Id)
                    .Contains(Id))
                .ToList();

            foreach (var specificationOption in SpecificationOptions)
            {
                specificationOption.Products.Remove(this);
            }
            SpecificationOptions.RemoveAll(sOp => true);
            context.SaveChanges();

            SpecificationOptions.AddRange(context
                .SpecificationOption
                .Where(sOp => specificationOptions.Contains(sOp.Id)));

            return this;
        }
        
        public Product()
        {
        }
    }
}