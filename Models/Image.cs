namespace Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        
        public Product Product { get; set; }
    }
}