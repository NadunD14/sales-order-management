using Microsoft.EntityFrameworkCore;
using SalesOrderManagement.Domain.Entities;

namespace SalesOrderManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address1).HasMaxLength(200);
                entity.Property(e => e.Address2).HasMaxLength(200);
                entity.Property(e => e.Address3).HasMaxLength(200);
                entity.Property(e => e.Suburb).HasMaxLength(100);
                entity.Property(e => e.State).HasMaxLength(100);
                entity.Property(e => e.PostCode).HasMaxLength(20);
            });

            // Configure Item
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ItemCode).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.ItemCode).IsUnique();
            });

            // Configure SalesOrder
            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ReferenceNo).HasMaxLength(100);
                entity.Property(e => e.Note).HasMaxLength(1000);
                entity.Property(e => e.Address1).HasMaxLength(200);
                entity.Property(e => e.Address2).HasMaxLength(200);
                entity.Property(e => e.Address3).HasMaxLength(200);
                entity.Property(e => e.Suburb).HasMaxLength(100);
                entity.Property(e => e.State).HasMaxLength(100);
                entity.Property(e => e.PostCode).HasMaxLength(20);
                
                entity.HasOne(e => e.Client)
                    .WithMany(c => c.SalesOrders)
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasIndex(e => e.InvoiceNo).IsUnique();
            });

            // Configure SalesOrderItem
            modelBuilder.Entity<SalesOrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Note).HasMaxLength(500);
                
                entity.HasOne(e => e.SalesOrder)
                    .WithMany(so => so.SalesOrderItems)
                    .HasForeignKey(e => e.SalesOrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Item)
                    .WithMany(i => i.SalesOrderItems)
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Clients
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, CustomerName = "John Smith", Address1 = "123 Main St", Address2 = "Suite 100", Suburb = "Downtown", State = "NY", PostCode = "10001" },
                new Client { Id = 2, CustomerName = "Sarah Johnson", Address1 = "456 Oak Ave", Suburb = "Westside", State = "CA", PostCode = "90210" },
                new Client { Id = 3, CustomerName = "Mike Davis", Address1 = "789 Pine Rd", Address2 = "Apt 5B", Suburb = "Easthill", State = "TX", PostCode = "75001" },
                new Client { Id = 4, CustomerName = "Emily Wilson", Address1 = "321 Elm St", Suburb = "Northtown", State = "FL", PostCode = "33101" },
                new Client { Id = 5, CustomerName = "David Brown", Address1 = "654 Maple Dr", Suburb = "Southpark", State = "WA", PostCode = "98101" }
            );

            // Seed Items
            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, ItemCode = "LAP001", Description = "Dell Laptop 15-inch", Price = 899.99m },
                new Item { Id = 2, ItemCode = "MOU001", Description = "Wireless Mouse", Price = 29.99m },
                new Item { Id = 3, ItemCode = "KEY001", Description = "Mechanical Keyboard", Price = 89.99m },
                new Item { Id = 4, ItemCode = "MON001", Description = "27-inch Monitor", Price = 299.99m },
                new Item { Id = 5, ItemCode = "HDD001", Description = "External Hard Drive 1TB", Price = 79.99m },
                new Item { Id = 6, ItemCode = "SPK001", Description = "Bluetooth Speakers", Price = 59.99m },
                new Item { Id = 7, ItemCode = "CAM001", Description = "Webcam HD", Price = 49.99m },
                new Item { Id = 8, ItemCode = "CHR001", Description = "USB-C Charger", Price = 39.99m },
                new Item { Id = 9, ItemCode = "TAB001", Description = "Graphics Tablet", Price = 199.99m },
                new Item { Id = 10, ItemCode = "PRT001", Description = "Wireless Printer", Price = 149.99m }
            );
        }
    }
}