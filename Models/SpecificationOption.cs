using System.Collections.Generic;

namespace Models
{
    public class SpecificationOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Specification Specification { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}