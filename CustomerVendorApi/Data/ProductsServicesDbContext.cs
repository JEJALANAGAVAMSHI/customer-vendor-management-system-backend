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
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Event> Events { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Business>()
            .HasKey(b => b.BusinessId);

            modelBuilder.Entity<Business>()
                .HasOne(b => b.Vendor)
                .WithMany(v => v.Businesses)
                .HasForeignKey(b => b.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Business>()
                .Property(b => b.BusinessName)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.Category)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.Address)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.State)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.PostalCode)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.Email)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.DayFrom)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.DayTo)
                .IsRequired();

            modelBuilder.Entity<Business>()
                .Property(b => b.TimeFrom)
                .HasColumnType("time");

            modelBuilder.Entity<Business>()
                .Property(b => b.TimeTo)
                .HasColumnType("time");

            // Service entity configuration
            modelBuilder.Entity<Service>()
                .HasKey(s => s.ServiceId);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.Business)
                .WithMany(b => b.Services)
                .HasForeignKey(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Service>()
                .Property(s => s.ServiceName)
                .IsRequired();

            modelBuilder.Entity<Service>()
                .Property(s => s.Description)
                .IsRequired();

            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            // Product entity configuration
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Business)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductName)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Vendor entity configuration
            modelBuilder.Entity<Vendor>()
                .HasKey(v => v.VendorId);

            modelBuilder.Entity<Vendor>()
                .Property(v => v.UserName)
                .IsRequired();

            modelBuilder.Entity<Vendor>()
                .HasMany(v => v.Businesses)
                .WithOne(b => b.Vendor)
                .HasForeignKey(b => b.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
               .HasKey(e => e.EventId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Business)
                .WithMany(b => b.Events)
                .HasForeignKey(e => e.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .Property(e => e.EventName)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .Property(e => e.Description)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .Property(e => e.Date)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .Property(e => e.TimeFrom)
                .HasColumnType("time");

            modelBuilder.Entity<Event>()
                .Property(e => e.TimeTo)
                .HasColumnType("time");

        }
    }
}
