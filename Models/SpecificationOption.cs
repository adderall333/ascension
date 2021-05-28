using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Attributes;

namespace Models
{
    public class SpecificationOption : IModel, ICategorized
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
        public List<Product> Products { get; set; }
        
        [NotMapped]
        [NotAdministered]
        public string CategoryName { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name} (Specification Id: {SpecificationId})";
        }
        
        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            var model = context
                .SpecificationOption
                .Include(specificationOption => specificationOption.Specification)
                .ThenInclude(specification => specification.Category)
                .Include(specificationOption => specificationOption.Products)
                .First(specificationOption => specificationOption.Id == id);
            model.CategoryName = model.Specification.Category.Name;
            return model;
        }
        
        public SpecificationOption Update(string name, int specification, List<int> products, ApplicationContext context)
        {
            Name = name;
            
            if (specification > 0)
                Specification = context.Specification.First(s => s.Id == specification);

            
            Products = context
                .Product
                .Include(p => p.SpecificationOptions)
                .Where(p => p
                    .SpecificationOptions
                    .Select(sOp => sOp.Id)
                    .Contains(Id))
                .ToList();
            
            foreach (var product in Products)
            {
                product.SpecificationOptions.Remove(this);
            }
            Products.RemoveAll(p => true);
            context.SaveChanges();
            
            Products.AddRange(context
                .Product
                .Where(product => products.Contains(product.Id)));
            
            return this;
        }

        public SpecificationOption()
        {
        }
    }
}