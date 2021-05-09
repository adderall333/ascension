using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using NpgsqlTypes;

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
        public IEnumerable<Purchase> Purchases { get; set; }
        
        public NpgsqlTsVector SearchVector { get; set; }
        
        public IEnumerable<Review> Reviews { get; set; }
        public ProductRating Rating { get; set; }

        [NotMapped]
        public bool IsInCart { get; set; }
    }
}