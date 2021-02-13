using System.Collections.Generic;

namespace Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public SuperCategory SuperCategory { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Specification> Specifications { get; set; }
    }
}