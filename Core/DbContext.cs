using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OmPlatform.Models;
using System.Reflection.Metadata;

namespace OmPlatform.Core
{
    public class DbAppContext : DbContext
    {
        public DbAppContext(DbContextOptions<DbAppContext> options) : base(options){}

        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Index foreign key
            modelBuilder.Entity<Orders>().HasIndex(o => o.UserId);
            modelBuilder.Entity<OrderItems>().HasIndex(oi => oi.ProductId);
            modelBuilder.Entity<OrderItems>().HasIndex(oi => oi.OrderId);
            // Index filtering
            modelBuilder.Entity<Products>().HasIndex(oi => oi.Category);

            // One to Many (User - Orders)
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Many to Many (OrderItems - Products & Orders)
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
