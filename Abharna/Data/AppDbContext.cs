using Abharna.Models;
using Microsoft.EntityFrameworkCore;

namespace Abharna.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Container> Containers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Container>().HasIndex(c => c.ContainerNumber).IsUnique();

            // starting data (the same demo containers as before)
            modelBuilder.Entity<Container>().HasData(
                new Container { Id = 1, ContainerNumber = "CMAU3300991", Cargo = "Pharmaceuticals", Stage = 1, Location = "Gate A-02", Vessel = "MV RED SEA", Eta = "20 Dec 2026" },
                new Container { Id = 2, ContainerNumber = "MAEU5556677", Cargo = "Automotive Parts", Stage = 2, Location = "At sea", Vessel = "MV AL MADINAH", Eta = "22 Dec 2026" },
                new Container { Id = 3, ContainerNumber = "ABHU1234567", Cargo = "Electronics & Consumer Goods", Stage = 3, Location = "Yard B-12", Vessel = "MV AL JAZIRA", Eta = "20 Dec 2026" },
                new Container { Id = 4, ContainerNumber = "ABHU7771234", Cargo = "Industrial Machinery", Stage = 4, Location = "Customs hold C-04", Vessel = "MV TIHAMA", Eta = "18 Dec 2026" },
                new Container { Id = 5, ContainerNumber = "MSCU8765432", Cargo = "Textile Bales", Stage = 5, Location = "Released", Vessel = "MV NOUR", Eta = "15 Dec 2026" }
            );
        }
    }
}