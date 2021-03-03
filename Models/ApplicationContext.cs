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
                optionsBuilder.UseNpgsql("Host=ec2-34-201-248-246.compute-1.amazonaws.com;Database=d93siims2cr0ac;Username=eycpyuyshcnrul;Password=4a936f7226c7a80e69a0981476e758cbe4dc27ad6d12c339c89735b2cb3eee30;sslmode=Require;TrustServerCertificate=true");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");
        }
    }
}