using System.Collections.Generic;
using System.Text.Json;

namespace Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<SpecificationOption> SpecificationOptions { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}