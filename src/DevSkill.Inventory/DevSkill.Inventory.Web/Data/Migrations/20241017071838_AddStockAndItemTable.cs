using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class AddStockAndItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Items",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					MeasurementUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					MeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					BuyingTaxCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					BuyingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					SellingTaxCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
					Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
					WarrantyDuration = table.Column<int>(type: "int", nullable: false),
					WarrantyUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
					IsActive = table.Column<bool>(type: "bit", nullable: false),
					IsSerialItem = table.Column<bool>(type: "bit", nullable: false),
					PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Items", x => x.Id);
					table.ForeignKey(
						name: "FK_Items_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Items_MeasurementUnits_MeasurementId",
						column: x => x.MeasurementId,
						principalTable: "MeasurementUnits",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Items_TaxCategories_BuyingTaxCategoryId",
						column: x => x.BuyingTaxCategoryId,
						principalTable: "TaxCategories",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Items_TaxCategories_SellingTaxCategoryId",
						column: x => x.SellingTaxCategoryId,
						principalTable: "TaxCategories",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "StockItems",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Quantity = table.Column<int>(type: "int", nullable: false),
					WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					AsOfDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_StockItems", x => x.Id);
					table.ForeignKey(
						name: "FK_StockItems_Items_ItemId",
						column: x => x.ItemId,
						principalTable: "Items",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade); // On delete cascade for Item
					table.ForeignKey(
						name: "FK_StockItems_Warehouses_WarehouseId",
						column: x => x.WarehouseId,
						principalTable: "Warehouses",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade); // On delete cascade for Warehouse
				});

			migrationBuilder.CreateIndex(
				name: "IX_Items_BuyingTaxCategoryId",
				table: "Items",
				column: "BuyingTaxCategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_Items_CategoryId",
				table: "Items",
				column: "CategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_Items_MeasurementId",
				table: "Items",
				column: "MeasurementId");

			migrationBuilder.CreateIndex(
				name: "IX_Items_SellingTaxCategoryId",
				table: "Items",
				column: "SellingTaxCategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_StockItems_ItemId",
				table: "StockItems",
				column: "ItemId");

			migrationBuilder.CreateIndex(
				name: "IX_StockItems_WarehouseId",
				table: "StockItems",
				column: "WarehouseId");

		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropTable(
			   name: "StockItems");

			migrationBuilder.DropTable(
				name: "Items");
		}
    }
}
