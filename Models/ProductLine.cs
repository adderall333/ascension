namespace Models
{
    public class ProductLine
    {
        public int Id { get; set; }
        
        public  int CartId { get; set; }

        public int ProductId { get; set; }
        
        public int ProductCount { get; set; }


        public ProductLine(int cartId, int productId)
        {
            CartId = cartId;
            ProductId = productId;
            ProductCount = 1;
        }

        public ProductLine()
        {
        }
    }
}