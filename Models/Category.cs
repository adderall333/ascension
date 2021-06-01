using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;

namespace Models
{
    public class Category : IModel, ICategory
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
        
        [SimpleProperty]
        public bool IsAvailable { get; set; }
        
        [ImageProperty]
        public string ImagePath { get; set; }
        
        [NotAdministered]
        public int SuperCategoryId { get; set; }
        
        [ManyToOne]
        public SuperCategory? SuperCategory { get; set; }
        
        [OneToMany]
        public List<Product> Products { get; set; }
        
        [OneToMany]
        public List<Specification> Specifications { get; set; }
        
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
        
        public Category Update(string name, string imagePath, int superCategory, ApplicationContext context)
        {
            Name = name;
            ImagePath = string.IsNullOrEmpty(imagePath) ? ImagePath : imagePath;
            
            if (superCategory > 0)
                SuperCategory = context.SuperCategory?.First(sc => sc.Id == superCategory);

            return this;
        }
        
        public Category()
        {
        }
    }
}