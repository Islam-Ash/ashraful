using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
	/// <inheritdoc />
	public partial class UpdateItemStockItemTabel : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Items_MeasurementUnits_MeasurementId",
				table: "Items");

			migrationBuilder.DropForeignKey(
				name: "FK_Items_TaxCategories_BuyingTaxCategoryId",
				table: "Items");

			migrationBuilder.DropForeignKey(
				name: "FK_Items_TaxCategories_SellingTaxCategoryId",
				table: "Items");

			migrationBuilder.DropIndex(
				name: "IX_Items_MeasurementId",
				table: "Items");

			migrationBuilder.DropColumn(
				name: "MeasurementId",
				table: "Items");

			migrationBuilder.AddColumn<decimal>(
				name: "CostPerUnit",
				table: "StockItems",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m);

			migrationBuilder.AlterColumn<Guid>(
				name: "SellingTaxCategoryId",
				table: "Items",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);

			migrationBuilder.AlterColumn<decimal>(
				name: "SellingPrice",
				table: "Items",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)",
				oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
				name: "BuyingTaxCategoryId",
				table: "Items",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Items_MeasurementUnitId",
				table: "Items",
				column: "MeasurementUnitId");

			migrationBuilder.AddForeignKey(
				name: "FK_Items_MeasurementUnits_MeasurementUnitId",
				table: "Items",
				column: "MeasurementUnitId",
				principalTable: "MeasurementUnits",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Items_TaxCategories_BuyingTaxCategoryId",
				table: "Items",
				column: "BuyingTaxCategoryId",
				principalTable: "TaxCategories",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction); // Change to NoAction

			migrationBuilder.AddForeignKey(
				name: "FK_Items_TaxCategories_SellingTaxCategoryId",
				table: "Items",
				column: "SellingTaxCategoryId",
				principalTable: "TaxCategories",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction); // Change to NoAction
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Items_MeasurementUnits_MeasurementUnitId",
				table: "Items");

			migrationBuilder.DropForeignKey(
				name: "FK_Items_TaxCategories_BuyingTaxCategoryId",
				table: "Items");

			migrationBuilder.DropForeignKey(
				name: "FK_Items_TaxCategories_SellingTaxCategoryId",
				table: "Items");

			migrationBuilder.DropIndex(
				name: "IX_Items_MeasurementUnitId",
				table: "Items");

			migrationBuilder.DropColumn(
				name: "CostPerUnit",
				table: "StockItems");

			migrationBuilder.AlterColumn<Guid>(
				name: "SellingTaxCategoryId",
				table: "Items",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AlterColumn<decimal>(
				name: "SellingPrice",
				table: "Items",
				type: "decimal(18,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)");

			migrationBuilder.AlterColumn<Guid>(
				name: "BuyingTaxCategoryId",
				table: "Items",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AddColumn<Guid>(
				name: "MeasurementId",
				table: "Items",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

			migrationBuilder.CreateIndex(
				name: "IX_Items_MeasurementId",
				table: "Items",
				column: "MeasurementId");

			migrationBuilder.AddForeignKey(
				name: "FK_Items_MeasurementUnits_MeasurementId",
				table: "Items",
				column: "MeasurementId",
				principalTable: "MeasurementUnits",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Items_TaxCategories_BuyingTaxCategoryId",
				table: "Items",
				column: "BuyingTaxCategoryId",
				principalTable: "TaxCategories",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Items_TaxCategories_SellingTaxCategoryId",
				table: "Items",
				column: "SellingTaxCategoryId",
				principalTable: "TaxCategories",
				principalColumn: "Id");
		}
	}
}
