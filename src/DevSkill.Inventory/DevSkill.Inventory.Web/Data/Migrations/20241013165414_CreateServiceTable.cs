using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class CreateServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("07ce3e79-5487-4bbb-aece-83c20a094bc2"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("0dc629f4-ce04-42e6-9fec-d9a7497c337c"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("2dc7fb7d-88c9-47bd-8121-447adf7bd76f"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("4e7e89e2-ff4a-44e6-84e3-8ecf6b8a81a7"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("54c8bbfe-5eb2-4f12-842f-c9ca2e0fdcfd"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("58fa5ac8-88b8-4274-a33c-405d58816cf8"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("7a90869f-94f6-46e0-b814-347d8f467beb"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("96f17476-35fc-4a6e-b7fe-8eab09939ef2"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("bb1a9e26-a562-4961-b6bc-53d212c1d006"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("d1d00b3d-3aad-4495-8ef7-bacd23d35a04"));

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false),
                    IsSelling = table.Column<bool>(type: "bit", nullable: false),
                    IsBuyingPriceTaxInclusive = table.Column<bool>(type: "bit", nullable: false),
                    IsSellingPriceTaxInclusive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_TaxCategories_TaxCategoryId",
                        column: x => x.TaxCategoryId,
                        principalTable: "TaxCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Services_TaxCategoryId",
                table: "Services",
                column: "TaxCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

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

            migrationBuilder.InsertData(
                table: "TaxCategories",
                columns: new[] { "Id", "Name", "Percentage" },
                values: new object[,]
                {
                    { new Guid("07ce3e79-5487-4bbb-aece-83c20a094bc2"), "Tax Free", 0m },
                    { new Guid("0dc629f4-ce04-42e6-9fec-d9a7497c337c"), "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)", 5m },
                    { new Guid("2dc7fb7d-88c9-47bd-8121-447adf7bd76f"), "VAT 13%", 13m },
                    { new Guid("4e7e89e2-ff4a-44e6-84e3-8ecf6b8a81a7"), "VAT 2% (Petrolium, Builders, etc.)", 2m },
                    { new Guid("54c8bbfe-5eb2-4f12-842f-c9ca2e0fdcfd"), "VAT 7% ", 7m },
                    { new Guid("58fa5ac8-88b8-4274-a33c-405d58816cf8"), "VAT 15% (Restaurants, Services, etc)", 15m },
                    { new Guid("7a90869f-94f6-46e0-b814-347d8f467beb"), "VAT 10% (Maintainance Service, Transport Service, etc.)", 10m },
                    { new Guid("96f17476-35fc-4a6e-b7fe-8eab09939ef2"), "VAT 2.4% (Pharmaceuticals)", 2.4m },
                    { new Guid("bb1a9e26-a562-4961-b6bc-53d212c1d006"), "VAT 17.4%", 17.5m },
                    { new Guid("d1d00b3d-3aad-4495-8ef7-bacd23d35a04"), "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)", 7.5m }
                });
        }
    }
}
