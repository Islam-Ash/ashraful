﻿// <auto-generated />
using System;
using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    [DbContext(typeof(InventoryDbContext))]
    [Migration("20241028033542_UpdateGetItemsSP4")]
    partial class UpdateGetItemsSP4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("82c3e46a-5a3f-49a9-b061-d5039cf7c82b"),
                            Name = "Home Appliances"
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("BuyingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("BuyingTaxCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSerialItem")
                        .HasColumnType("bit");

                    b.Property<Guid>("MeasurementUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("SellingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("SellingTaxCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("WarrantyDuration")
                        .HasColumnType("int");

                    b.Property<string>("WarrantyUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BuyingTaxCategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("MeasurementUnitId");

                    b.HasIndex("SellingTaxCategoryId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Measurement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MeasurementName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MeasurementUnits");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("BuyingPriceTaxed")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBuyingPriceTaxInclusive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPurchased")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSellingPriceTaxInclusive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSold")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SellingPriceTaxed")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("TaxCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TaxCategoryId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AsOfDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("CostPerUnit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid>("WarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("StockItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.TaxCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("TaxCategories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5ee3319a-de86-461e-bcf0-db849cb2470a"),
                            Name = "Tax Free",
                            Percentage = 0m
                        },
                        new
                        {
                            Id = new Guid("b9f268d0-de43-4706-805d-f0cd6fff0c08"),
                            Name = "VAT 2% (Petrolium, Builders, etc.)",
                            Percentage = 2m
                        },
                        new
                        {
                            Id = new Guid("59e040c8-978a-4ca5-95f9-d71cde9af5b9"),
                            Name = "VAT 2.4% (Pharmaceuticals)",
                            Percentage = 2.4m
                        },
                        new
                        {
                            Id = new Guid("b1420c83-2c66-4de1-b8ba-7e846096409f"),
                            Name = "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)",
                            Percentage = 5m
                        },
                        new
                        {
                            Id = new Guid("76a385d1-e114-49c7-b27a-47fbd7364d39"),
                            Name = "VAT 7% ",
                            Percentage = 7m
                        },
                        new
                        {
                            Id = new Guid("ae0eac0f-e761-4647-91e1-085cbebaaa01"),
                            Name = "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)",
                            Percentage = 7.5m
                        },
                        new
                        {
                            Id = new Guid("5f12f844-e04a-41cd-a26f-f9564421b004"),
                            Name = "VAT 10% (Maintainance Service, Transport Service, etc.) ",
                            Percentage = 10m
                        },
                        new
                        {
                            Id = new Guid("ed7e68de-23a3-4bf0-b809-e9a9f528af38"),
                            Name = "VAT 13% ",
                            Percentage = 13m
                        },
                        new
                        {
                            Id = new Guid("48506eae-f491-4f1b-8dc7-2b14f4f3394d"),
                            Name = "VAT 15% (Restaurants, Services, etc) ",
                            Percentage = 15m
                        },
                        new
                        {
                            Id = new Guid("0ec04961-c3f3-432b-aebf-f020ff72b04b"),
                            Name = "VAT 17.4%",
                            Percentage = 17.5m
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Warehouse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a4561b2e-b388-4c3c-9062-011b9c9e0da7"),
                            Name = "Main"
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.TaxCategory", "BuyingTaxCategory")
                        .WithMany("BuyingItems")
                        .HasForeignKey("BuyingTaxCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Measurement", "MeasurementUnit")
                        .WithMany("Items")
                        .HasForeignKey("MeasurementUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.TaxCategory", "SellingTaxCategory")
                        .WithMany("SellingItems")
                        .HasForeignKey("SellingTaxCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BuyingTaxCategory");

                    b.Navigation("Category");

                    b.Navigation("MeasurementUnit");

                    b.Navigation("SellingTaxCategory");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Product", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Service", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.TaxCategory", "TaxCategory")
                        .WithMany()
                        .HasForeignKey("TaxCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaxCategory");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockItem", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Item")
                        .WithMany("StockItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "Warehouse")
                        .WithMany("StockItems")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Category", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.Navigation("StockItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Measurement", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.TaxCategory", b =>
                {
                    b.Navigation("BuyingItems");

                    b.Navigation("SellingItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Warehouse", b =>
                {
                    b.Navigation("StockItems");
                });
#pragma warning restore 612, 618
        }
    }
}
