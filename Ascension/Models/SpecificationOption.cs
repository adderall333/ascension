using System.Collections.Generic;

namespace Ascension.Models
{
    public class SpecificationOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Specification Specification { get; set; }
        public List<Product> Products { get; set; }
    }
}