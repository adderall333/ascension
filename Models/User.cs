using System.Linq;
using Models.Attributes;
using System.Collections.Generic;

namespace Models
{
    public class User : IModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Email { get; set; }
        
        public string HashedPassword { get; set; }
        
        public bool IsAdmin { get; set; }
        
        public IEnumerable<Review> Reviews { get; set; }
        
        [NotAdministered]
        public IEnumerable<Order> Orders { get; set; }

        public override string ToString()
        {
            return $"{Id}.{Name} {Surname}";
        }

        public User()
        {
        }
    }
}