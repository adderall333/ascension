namespace Models
{
    public class ProductLine
    {
        public int Id { get; set; }
        
        public  int ClientId { get; set; }

        public int ProductId { get; set; }
        
        public int ProductCount { get; set; }


        public ProductLine(int clientId, int productId)
        {
            ClientId = clientId;
            ProductId = productId;
            ProductCount = 1;
        }
    }
}