using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class AddSeedingInTaxCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("026701cc-a0dd-44ed-a332-aa9a1b2ea097"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("09cee29c-c1c6-4b14-8b9f-fe68ba32bb8e"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("2d213972-9b83-47c4-82d1-3bb90f7118b4"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("59ef2f89-1a1d-4fc8-b52b-cd59e6e9873d"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("8be01e4e-f0a7-4173-943e-c79787fbf1ea"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("a8960b86-0e63-4aac-a0f7-3decabab4c58"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("d003a158-1cf0-44ee-9bea-f6d41299d28d"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("d5a67506-7dac-45ab-a39f-e7dd7dac777e"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("ea4af7eb-1c5d-47c0-96e7-b8bf9a815d13"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("fee71fad-32ac-44c3-b4f9-eb59e24ecd2a"));

            migrationBuilder.InsertData(
                table: "TaxCategories",
                columns: new[] { "Id", "Name", "Percentage" },
                values: new object[,]
                {
                    { new Guid("0ec04961-c3f3-432b-aebf-f020ff72b04b"), "VAT 17.4%", 17.5m },
                    { new Guid("48506eae-f491-4f1b-8dc7-2b14f4f3394d"), "VAT 15% (Restaurants, Services, etc) ", 15m },
                    { new Guid("59e040c8-978a-4ca5-95f9-d71cde9af5b9"), "VAT 2.4% (Pharmaceuticals)", 2.4m },
                    { new Guid("5ee3319a-de86-461e-bcf0-db849cb2470a"), "Tax Free", 0m },
                    { new Guid("5f12f844-e04a-41cd-a26f-f9564421b004"), "VAT 10% (Maintainance Service, Transport Service, etc.) ", 10m },
                    { new Guid("76a385d1-e114-49c7-b27a-47fbd7364d39"), "VAT 7% ", 7m },
                    { new Guid("ae0eac0f-e761-4647-91e1-085cbebaaa01"), "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)", 7.5m },
                    { new Guid("b1420c83-2c66-4de1-b8ba-7e846096409f"), "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)", 5m },
                    { new Guid("b9f268d0-de43-4706-805d-f0cd6fff0c08"), "VAT 2% (Petrolium, Builders, etc.)", 2m },
                    { new Guid("ed7e68de-23a3-4bf0-b809-e9a9f528af38"), "VAT 13% ", 13m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("0ec04961-c3f3-432b-aebf-f020ff72b04b"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("48506eae-f491-4f1b-8dc7-2b14f4f3394d"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("59e040c8-978a-4ca5-95f9-d71cde9af5b9"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("5ee3319a-de86-461e-bcf0-db849cb2470a"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("5f12f844-e04a-41cd-a26f-f9564421b004"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("76a385d1-e114-49c7-b27a-47fbd7364d39"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("ae0eac0f-e761-4647-91e1-085cbebaaa01"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("b1420c83-2c66-4de1-b8ba-7e846096409f"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("b9f268d0-de43-4706-805d-f0cd6fff0c08"));

            migrationBuilder.DeleteData(
                table: "TaxCategories",
                keyColumn: "Id",
                keyValue: new Guid("ed7e68de-23a3-4bf0-b809-e9a9f528af38"));

            migrationBuilder.InsertData(
                table: "TaxCategories",
                columns: new[] { "Id", "Name", "Percentage" },
                values: new object[,]
                {
                    { new Guid("026701cc-a0dd-44ed-a332-aa9a1b2ea097"), "VAT 2% (Petrolium, Builders, etc.)", 2m },
                    { new Guid("09cee29c-c1c6-4b14-8b9f-fe68ba32bb8e"), "VAT 5% (Garments, Crockeries, Toiletries,Raw Material Tax, etc)", 5m },
                    { new Guid("2d213972-9b83-47c4-82d1-3bb90f7118b4"), "VAT 2.4% (Pharmaceuticals)", 2.4m },
                    { new Guid("59ef2f89-1a1d-4fc8-b52b-cd59e6e9873d"), "VAT 7% ", 7m },
                    { new Guid("8be01e4e-f0a7-4173-943e-c79787fbf1ea"), "VAT 17.4%", 17.5m },
                    { new Guid("a8960b86-0e63-4aac-a0f7-3decabab4c58"), "VAT 10% (Maintainance Service, Transport Service, etc.) ", 10m },
                    { new Guid("d003a158-1cf0-44ee-9bea-f6d41299d28d"), "VAT 15% (Restaurants, Services, etc) ", 15m },
                    { new Guid("d5a67506-7dac-45ab-a39f-e7dd7dac777e"), "Tax Free", 0m },
                    { new Guid("ea4af7eb-1c5d-47c0-96e7-b8bf9a815d13"), "VAT 13% ", 13m },
                    { new Guid("fee71fad-32ac-44c3-b4f9-eb59e24ecd2a"), "VAT 7.5% (Paper, Auctioning goods, Own Branded Garments, etc.)", 7.5m }
                });
        }
    }
}
