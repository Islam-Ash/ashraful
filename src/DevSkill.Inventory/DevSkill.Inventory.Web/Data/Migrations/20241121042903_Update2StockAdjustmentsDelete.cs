using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class Update2StockAdjustmentsDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Items_ItemId",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Items_ItemId",
                table: "StockItems");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Items_ItemId",
                table: "StockAdjustments",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Items_ItemId",
                table: "StockItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_Items_ItemId",
                table: "StockAdjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Items_ItemId",
                table: "StockItems");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Items_ItemId",
                table: "StockAdjustments",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Items_ItemId",
                table: "StockItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
