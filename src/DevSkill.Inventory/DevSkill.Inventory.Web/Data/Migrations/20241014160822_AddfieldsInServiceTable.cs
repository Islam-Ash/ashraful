using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class AddfieldsInServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("0a0ce45a-a5be-4f5c-8d4f-74d9a37bfbfc"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("2a1e8243-86e3-4f1f-a2c7-81d01eff49ed"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("52637ad8-6c6b-42cc-a48c-af7241f9fd17"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("5640f7ff-163b-4370-b7aa-d42ab04fab3c"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("6813ce3a-8654-4217-8b5c-cfda8ca5ba0d"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("75a218cd-5c91-4abd-b33e-3c7e05d4ed6a"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("8cddefd1-aafe-4d18-b95e-2d239d75dfc3"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("be6ba597-fda2-462e-a65c-7627967b87fc"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("d4acc359-7b71-4194-aeb3-16c30f5e90fb"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("f8d05303-6264-4df1-b2c3-bc323f988726"));

            migrationBuilder.RenameColumn(
                name: "IsSelling",
                table: "Services",
                newName: "IsSold");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "BuyingPriceTaxed",
                table: "Services",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPriceTaxed",
                table: "Services",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "TaxCategories",
                columns: new[] { "Id", "Name", "Percentage" },
                values: new object[,]
                {
                    { new Guid("170dcd3c-60e0-4698-83f2-950857efa0f6"), "VAT 15% (Restaurants, Services, etc)", 15m },
                    { new Guid("19befb28-a19b-47c8-902f-2d8ee207b9d4"), "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)", 5m },
                    { new Guid("211e47da-b778-4095-bb8a-90849269d582"), "VAT 2.4% (Pharmaceuticals)", 2.4m },
                    { new Guid("3b1da476-7180-42e0-a556-b9fa5c7714d7"), "VAT 7% ", 7m },
                    { new Guid("4ce22143-6a7e-41c0-8147-63ded0bc8d20"), "Tax Free", 0m },
                    { new Guid("826c4b39-52f0-40e8-a817-1f4c6bcf0b80"), "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)", 7.5m },
                    { new Guid("86f1409f-6158-4b61-a5cd-f3573a76b029"), "VAT 2% (Petrolium, Builders, etc.)", 2m },
                    { new Guid("9f8d83b7-236e-4546-a680-ac8ab0cb1bb4"), "VAT 13%", 13m },
                    { new Guid("b0a7e643-6905-4e26-858d-500cac91cb4e"), "VAT 10% (Maintainance Service, Transport Service, etc.)", 10m },
                    { new Guid("d98011b5-b9ee-4fca-ae36-fd2485232133"), "VAT 17.4%", 17.5m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("170dcd3c-60e0-4698-83f2-950857efa0f6"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("19befb28-a19b-47c8-902f-2d8ee207b9d4"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("211e47da-b778-4095-bb8a-90849269d582"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("3b1da476-7180-42e0-a556-b9fa5c7714d7"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("4ce22143-6a7e-41c0-8147-63ded0bc8d20"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("826c4b39-52f0-40e8-a817-1f4c6bcf0b80"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("86f1409f-6158-4b61-a5cd-f3573a76b029"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("9f8d83b7-236e-4546-a680-ac8ab0cb1bb4"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("b0a7e643-6905-4e26-858d-500cac91cb4e"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("d98011b5-b9ee-4fca-ae36-fd2485232133"));

            migrationBuilder.DropColumn(
                name: "BuyingPriceTaxed",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "SellingPriceTaxed",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "IsSold",
                table: "Services",
                newName: "IsSelling");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "TaxCategories",
                columns: new[] { "Id", "Name", "Percentage" },
                values: new object[,]
                {
                    { new Guid("0a0ce45a-a5be-4f5c-8d4f-74d9a37bfbfc"), "VAT 15% (Restaurants, Services, etc)", 15m },
                    { new Guid("2a1e8243-86e3-4f1f-a2c7-81d01eff49ed"), "VAT 7% ", 7m },
                    { new Guid("52637ad8-6c6b-42cc-a48c-af7241f9fd17"), "VAT 2.4% (Pharmaceuticals)", 2.4m },
                    { new Guid("5640f7ff-163b-4370-b7aa-d42ab04fab3c"), "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)", 5m },
                    { new Guid("6813ce3a-8654-4217-8b5c-cfda8ca5ba0d"), "VAT 17.4%", 17.5m },
                    { new Guid("75a218cd-5c91-4abd-b33e-3c7e05d4ed6a"), "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)", 7.5m },
                    { new Guid("8cddefd1-aafe-4d18-b95e-2d239d75dfc3"), "VAT 10% (Maintainance Service, Transport Service, etc.)", 10m },
                    { new Guid("be6ba597-fda2-462e-a65c-7627967b87fc"), "Tax Free", 0m },
                    { new Guid("d4acc359-7b71-4194-aeb3-16c30f5e90fb"), "VAT 2% (Petrolium, Builders, etc.)", 2m },
                    { new Guid("f8d05303-6264-4df1-b2c3-bc323f988726"), "VAT 13%", 13m }
                });
        }
    }
}
