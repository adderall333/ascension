using System.Linq;
using Models.Attributes;

namespace Models
{
    public class User : IModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        
        [SimpleProperty]
        public string Name { get; set; }
        
        [SimpleProperty]
        public string Surname { get; set; }
        
        [SimpleProperty]
        public string Email { get; set; }
        
        [NotAdministered]
        public string HashedPassword { get; set; }
        
        [SimpleProperty]
        public bool IsAdmin { get; set; }

        public override string ToString()
        {
            return $"{Id}.{Name} {Surname}";
        }

        public static IModel GetModel(int id)
        {
            var context = new ApplicationContext();
            return context
                .User
                .First(u => u.Id == id);
        }

        public User(string name, string surname, string email, bool isAdmin)
        {
            Name = name;
            Surname = surname;
            Email = email;
            IsAdmin = isAdmin;
        }

        public User()
        {
        }
    }
}