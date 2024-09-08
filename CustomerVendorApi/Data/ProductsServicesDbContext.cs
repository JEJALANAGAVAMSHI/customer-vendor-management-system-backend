using CustomerVendorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerVendorApi.Data
{
    public class ProductsServicesDbContext : DbContext
    {
        public ProductsServicesDbContext(DbContextOptions<ProductsServicesDbContext> options)
            : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
