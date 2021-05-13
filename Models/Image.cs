using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Attributes;

namespace Models
{
    public class Image : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [ImageProperty]
        public string Path { get; set; }
        
        [NotAdministered]
        public int ProductId { get; set; }
        
        [ManyToOne]
        public Product Product { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Path}";
        }

        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            return context
                .Image
                .Include(image => image.Product)
                .First(image => image.Id == id);
        }
        
        public Image Update(string path, int product, ApplicationContext context)
        {
            Path = string.IsNullOrEmpty(path) ? Path : path; 
            
            if (product > 0)
                Product = context.Product.First(p => p.Id == product);

            return this;
        }

        public Image()
        {
        }
    }
}