using System;

namespace Models
{
    public class Review
    {
        public int Id { get; set; }
        
        public string? Comment { get; set; }
        
        public int Rating { get; set; }
        
        public DateTime PublicationDate { get; set; }
        
        public User? User { get; set; }
        
        public Product? Product { get; set; }
    }
}