using System.Collections.Generic;
using System.Text.Json;

namespace Ascension.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public JsonDocument Specifications { get; set; }
        
        public Category Category { get; set; }
        public List<SpecificationOption> SpecificationOptions { get; set; }
    }
}