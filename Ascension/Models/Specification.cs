using System.Collections.Generic;

namespace Ascension.Models
{
    public class Specification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public Category Category { get; set; }
        public List<SpecificationOption> SpecificationOptions { get; set; }
    }
}