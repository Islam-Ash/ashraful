using DevSkill.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DevSkill.Inventory.Infrastructure
{
    public class InventoryDbContext :DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public InventoryDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Item>()
		        .HasOne(i => i.Category)
		        .WithMany(c => c.Items)
		        .HasForeignKey(i => i.CategoryId);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.MeasurementUnit)
				.WithMany(m => m.Items)
				.HasForeignKey(i => i.MeasurementUnitId);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.BuyingTaxCategory)
				.WithMany(t => t.BuyingItems)
				.HasForeignKey(i => i.BuyingTaxCategoryId);

			modelBuilder.Entity<Item>()
				.HasOne(i => i.SellingTaxCategory)
				.WithMany(t => t.SellingItems)
				.HasForeignKey(i => i.SellingTaxCategoryId);

			modelBuilder.Entity<StockItem>()
				.HasOne(s => s.Item)
				.WithMany(i => i.StockItems)
				.HasForeignKey(s => s.ItemId);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(s => s.SourceWarehouse)
                .WithMany()
                .HasForeignKey(s => s.SourceWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(s => s.DestinationWarehouse)
                .WithMany()
                .HasForeignKey(s => s.DestinationWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockTransfer>()
                .HasOne(s => s.Item)
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockAdjustment>()
                .HasOne(sa => sa.Item)
                .WithMany()
                .HasForeignKey(sa => sa.ItemId)
                .OnDelete(DeleteBehavior.Restrict);  // Disable cascade delete


            modelBuilder.Entity<StockAdjustment>()
                .HasOne(sa => sa.Reason) // Each adjustment has a reason
                .WithMany() // A reason can be used in multiple adjustments
                .HasForeignKey(sa => sa.ReasonId);

            modelBuilder.Entity<StockItem>()
                .HasOne(si => si.Item)
                .WithMany(i => i.StockItems)
                .HasForeignKey(si => si.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData(new Category 
            {
                Id = new Guid("82C3E46A-5A3F-49A9-B061-D5039CF7C82B"),
                Name = "Home Appliances",
			});

            modelBuilder.Entity<TaxCategory>().HasData(new[]
           {
                new TaxCategory { Id = new Guid("5EE3319A-DE86-461E-BCF0-DB849CB2470A"), Name = "Tax Free", Percentage = 0 },
                new TaxCategory { Id = new Guid("B9F268D0-DE43-4706-805D-F0CD6FFF0C08"), Name = "VAT 2% (Petrolium, Builders, etc.)", Percentage = 2 },
                new TaxCategory { Id = new Guid("59E040C8-978A-4CA5-95F9-D71CDE9AF5B9"), Name = "VAT 2.4% (Pharmaceuticals)", Percentage = 2.4M },
                new TaxCategory { Id = new Guid("B1420C83-2C66-4DE1-B8BA-7E846096409F"), Name = "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)", Percentage = 5 },
                new TaxCategory { Id = new Guid("76A385D1-E114-49C7-B27A-47FBD7364D39"), Name = "VAT 7% ", Percentage = 7 },
                new TaxCategory { Id = new Guid("AE0EAC0F-E761-4647-91E1-085CBEBAAA01"), Name = "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)", Percentage = 7.5M },
                new TaxCategory { Id = new Guid("5F12F844-E04A-41CD-A26F-F9564421B004"), Name = "VAT 10% (Maintainance Service, Transport Service, etc.) ", Percentage = 10 },
                new TaxCategory { Id = new Guid("ED7E68DE-23A3-4BF0-B809-E9A9F528AF38"), Name = "VAT 13% ", Percentage = 13 },
                new TaxCategory { Id = new Guid("48506EAE-F491-4F1B-8DC7-2B14F4F3394D"), Name = "VAT 15% (Restaurants, Services, etc) ", Percentage = 15 },
                new TaxCategory { Id = new Guid("0EC04961-C3F3-432B-AEBF-F020FF72B04B"), Name = "VAT 17.4%", Percentage = 17.5M },

            });

            modelBuilder.Entity<Warehouse>().HasData(new Warehouse
            {
                Id = new Guid("A4561B2E-B388-4C3C-9062-011B9C9E0DA7"),
                Name = "Main",
            });

            // Set precision for decimal properties
            modelBuilder.Entity<Item>()
                .Property(i => i.BuyingPrice)
                .HasColumnType("decimal(18, 4)"); // Allow precision up to 4 decimal places
            modelBuilder.Entity<Item>()
                .Property(i => i.SellingPrice)
                .HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<Service>()
                .Property(s => s.BuyingPriceTaxed)
                .HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<Service>()
                .Property(s => s.SellingPriceTaxed)
                .HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<StockItem>()
                .Property(si => si.CostPerUnit)
                .HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<TaxCategory>()
                .Property(tc => tc.Percentage)
                .HasColumnType("decimal(18, 4)");

            base.OnModelCreating(modelBuilder);
		}

		public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Measurement> MeasurementUnits { get; set; }
        public DbSet<TaxCategory> TaxCategories { get; set; }
        public DbSet<Service> Services {  get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<Reason> Reasons { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
    }
}
