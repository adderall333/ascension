using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<SpecificationOption> SpecificationOption { get; set; }
        public DbSet<Specification> Specification { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SuperCategory> SuperCategory { get; set; }
        public DbSet<Image> Image { get; set; }
        
        
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=ascension_db;Username=postgres;Password=qweasd123");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");
        }
    }
}