using System.Collections.Generic;

namespace Models
{
    public class SuperCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
                
        public IEnumerable<Category> Categories { get; set; }
    }
}