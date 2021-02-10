using System.Collections.Generic;

namespace Ascension.Models
{
    public class SuperCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
                
        public List<Category> Categories { get; set; }
    }
}