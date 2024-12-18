using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class Update2StockAdjustmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockAdjustments_StockItems_StockItemId",
                table: "StockAdjustments");

            migrationBuilder.RenameColumn(
                name: "StockItemId",
                table: "StockAdjustments",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustments_StockItemId",
                table: "StockAdjustments",
                newName: "IX_StockAdjustments_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_Items_ItemId",
                table: "StockAdjustments",
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

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "StockAdjustments",
                newName: "StockItemId");

            migrationBuilder.RenameIndex(
                name: "IX_StockAdjustments_ItemId",
                table: "StockAdjustments",
                newName: "IX_StockAdjustments_StockItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockAdjustments_StockItems_StockItemId",
                table: "StockAdjustments",
                column: "StockItemId",
                principalTable: "StockItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
