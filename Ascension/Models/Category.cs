using System.Collections.Generic;

namespace Ascension.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public SuperCategory SuperCategory { get; set; }
        public List<Product> Products { get; set; }
        public List<Specification> Specifications { get; set; }
    }
}