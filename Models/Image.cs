namespace Models
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Bytes { get; set; }
        
        public Product Product { get; set; }
    }
}