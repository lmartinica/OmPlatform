using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OmPlatform.Models;
using System.Reflection.Metadata;

namespace OmPlatform.Core
{
    public class DbAppContext : DbContext
    {
        public DbAppContext(DbContextOptions<DbAppContext> options): base(options){ }

        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=OmPlatform;Trusted_Connection=True;TrustServerCertificate=True");
    }

}
