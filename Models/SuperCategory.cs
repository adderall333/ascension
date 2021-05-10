using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;

namespace Models
{
    public class SuperCategory : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
        
        [OneToOne]
        public Image Image { get; set; }
                
        [OneToMany]
        public IEnumerable<Category> Categories { get; set; }
        
        public override string ToString()
        {
            return $"{Id}.{Name}";
        }
        
        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            return context
                .SuperCategory
                .Include(superCategory => superCategory.Categories)
                .Include(superCategory => superCategory.Image)
                .First(superCategory => superCategory.Id == id);
        }

        public SuperCategory(string name, int image, List<int> categories, ApplicationContext context = null)
        {
            Name = name;
            
            if (image > 0)
                Image = context?.Image.First(i => i.Id == image);
            
            if (categories.Any())
                Categories = context?.Category.Where(category => categories.Contains(category.Id)).ToList();
        }

        public void Update(string name, int image, List<int> categories, ApplicationContext context = null)
        {
            Name = name;
            
            if (image > 0)
                Image = context?.Image.First(i => i.Id == image);
            
            if (categories.Any())
                Categories = context?.Category.Where(category => categories.Contains(category.Id)).ToList();
        }

        public SuperCategory()
        {
        }
    }
}