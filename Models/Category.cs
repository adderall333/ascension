using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;

namespace Models
{
    public class Category : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
        
        //todo add image
        
        [ManyToOne]
        public SuperCategory SuperCategory { get; set; }
        
        [OneToMany]
        public IEnumerable<Product> Products { get; set; }
        
        [OneToMany]
        public IEnumerable<Specification> Specifications { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name}";
        }
        
        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            return context
                .Category
                .Include(category => category.SuperCategory)
                .Include(category => category.Products)
                .Include(category => category.Specifications)
                .First(category => category.Id == id);
        }

        public Category(string name, int superCategory, List<int> products, List<int> specifications, ApplicationContext context = null)
        {
            Name = name;
            
            if (superCategory > 0)
                SuperCategory = context?.SuperCategory.First(sc => sc.Id == superCategory);
            
            if (products.Any())
                Products = context?.Product.Where(product => products.Contains(product.Id)).ToList();
            
            if (specifications.Any())
                Specifications = context?.Specification.Where(specification => specifications.Contains(specification.Id));
        }

        public Category()
        {
        }
    }
}