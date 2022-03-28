using Microsoft.EntityFrameworkCore;

namespace Restaurant2API.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\MSSQLLocalDB;database=RestaurantDb;Trusted_Connection=True;";
       
        public DbSet<Restaurant> Restaurants { get; set;}
        public DbSet<Address> Addresses { get; set;}
        public DbSet<Dish> Dishes { get; set;}

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(28);

            modelBuilder.Entity<Restaurant>()
                .HasOne(a => a.Address)
                .WithOne(a => a.Restaurant)
                .HasForeignKey<Restaurant>(c => c.AdressId);

            modelBuilder.Entity<Dish>()
                .Property(d => d.Name)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(a=>a.City)
                .IsRequired(true)
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired(true)
                .HasMaxLength(50);

            modelBuilder.Entity<Role>()
                .Property(a => a.Name)
                .IsRequired(true);

            modelBuilder.Entity<User>()
              .Property(a => a.Emali)
                .IsRequired(true);


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}
