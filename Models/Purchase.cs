using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int Count { get; set; }
        
        [InverseProperty("Purchases")]
        public Product FirstProduct { get; set; }
        public int FirstProductId { get; set; }
        
        public Product SecondProduct { get; set; }
        public int SecondProductId { get; set; }
    }
}