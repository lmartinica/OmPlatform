using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OmPlatform.Models;
using System.Reflection.Metadata;

namespace OmPlatform.Core
{
    public class DbAppContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbAppContext(DbContextOptions<DbAppContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        // TODO: check required columns, nvarchar(50)

        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }

}
