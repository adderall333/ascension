using System.Collections.Generic;

namespace Models
{
    public class Specification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<SpecificationOption> SpecificationOptions { get; set; }
    }
}