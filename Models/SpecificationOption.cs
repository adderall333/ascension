using System.Collections.Generic;
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
        public List<Product> Products { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name} (Specification Id: {SpecificationId})";
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