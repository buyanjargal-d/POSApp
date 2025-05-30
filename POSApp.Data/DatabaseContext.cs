using Microsoft.EntityFrameworkCore;
using POSApp.Core.Models;

namespace POSApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure enum to int mapping for Role
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>();

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "manager", PasswordHash = "manager123", Role = Role.Manager },
                new User { Id = 2, Username = "cashier1", PasswordHash = "cashier123", Role = Role.Cashier },
                new User { Id = 3, Username = "cashier2", PasswordHash = "cashier123", Role = Role.Cashier }
            );

            // Seed Items
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Code = "A001", Name = "Water Bottle", UnitPrice = 1.99m, Category = "Drinks", ImagePath = "images/water.png"},
                new Item { Id = 2, Code = "A002", Name = "Notebook", UnitPrice = 3.49m, Category = "Others", ImagePath = "images/water.png" },
                new Item { Id = 3, Code = "A003", Name = "Pen", UnitPrice = 0.99m, Category = "Others", ImagePath = "images/water.png" },
                new Item { Id = 4, Code = "A004", Name = "Burger", UnitPrice = 1.99m, Category = "Foods", ImagePath = "images/water.png" },
                new Item { Id = 5, Code = "A005", Name = "Cup", UnitPrice = 3.49m, Category = "Others", ImagePath = "images/water.png" },
                new Item { Id = 6, Code = "A006", Name = "Test", UnitPrice = 0.99m, Category = "Test", ImagePath = "images/water.png" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
