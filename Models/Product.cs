using System.Collections.Generic;
using System.Text.Json;

namespace Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public JsonDocument Specifications { get; set; }
        
        public Category Category { get; set; }
        public IEnumerable<SpecificationOption> SpecificationOptions { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}