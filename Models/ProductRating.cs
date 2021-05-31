namespace Models
{
    public class ProductRating
    {
        public int Id { get; set; }
        
        public int Sum { get; set; }
        
        public int Count { get; set; }
        
        public int ProductId { get; set; }
        
        public Product? Product { get; set; }
    }
}