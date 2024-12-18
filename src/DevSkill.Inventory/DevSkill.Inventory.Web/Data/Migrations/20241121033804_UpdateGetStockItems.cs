using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class UpdateGetStockItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetStockItems
                    @PageIndex INT,
                    @PageSize INT, 
                    @OrderBy NVARCHAR(50),
                    @ItemName NVARCHAR(MAX) = NULL,  
                    @WarehouseId UNIQUEIDENTIFIER = NULL, 
                    @Barcode NVARCHAR(MAX) = NULL,
                    @CategoryId UNIQUEIDENTIFIER = NULL,
                    @Total INT OUTPUT,
                    @TotalDisplay INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- Trim input parameters
                    SET @ItemName = LTRIM(RTRIM(@ItemName));
                    SET @Barcode = LTRIM(RTRIM(@Barcode));

                    -- Ensure @ItemName and @Barcode are properly handled for wildcards
                    IF @ItemName IS NULL OR @ItemName = '' SET @ItemName = '%';
                    ELSE SET @ItemName = '%' + @ItemName + '%';

                    IF @Barcode IS NULL OR @Barcode = '' SET @Barcode = '%';
                    ELSE SET @Barcode = '%' + @Barcode + '%';

                    -- Collect Total Count
                    SELECT @Total = COUNT(*) FROM StockItems;

                    -- Collect Total Display Count (filtered data)
                    SELECT @TotalDisplay = COUNT(*)
                    FROM StockItems stock
                    LEFT JOIN Items i ON stock.ItemId = i.Id
                    LEFT JOIN Categories c ON i.CategoryId = c.Id
                    LEFT JOIN Warehouses w ON stock.WarehouseId = w.Id
                    WHERE LOWER(i.Name) LIKE LOWER(@ItemName)
                      AND LOWER(i.Barcode) LIKE LOWER(@Barcode)
                      AND (@WarehouseId IS NULL OR stock.WarehouseId = @WarehouseId)
                      AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId);

                    -- Main Query with Pagination
                    WITH StockData AS
                    (
                        SELECT stock.Id, i.Name AS ItemName, i.Barcode, 
                               w.Name AS WarehouseName, c.Name AS CategoryName, 
                               stock.Quantity, stock.CostPerUnit, stock.AsOfDate
                        FROM StockItems stock
                        LEFT JOIN Items i ON stock.ItemId = i.Id
                        LEFT JOIN Categories c ON i.CategoryId = c.Id
                        LEFT JOIN Warehouses w ON stock.WarehouseId = w.Id
                        WHERE LOWER(i.Name) LIKE LOWER(@ItemName)
                          AND LOWER(i.Barcode) LIKE LOWER(@Barcode)
                          AND (@WarehouseId IS NULL OR stock.WarehouseId = @WarehouseId)
                          AND (@CategoryId IS NULL OR i.CategoryId = @CategoryId)
                    )
                    SELECT *
                    FROM StockData
                    ORDER BY 
                        CASE 
                            WHEN @OrderBy = 'ItemName' THEN ItemName
                            WHEN @OrderBy = 'Barcode' THEN Barcode
                            WHEN @OrderBy = 'CategoryName' THEN CategoryName
                            WHEN @OrderBy = 'WarehouseName' THEN WarehouseName
                            ELSE ItemName
                        END
                    OFFSET @PageSize * (@PageIndex - 1) ROWS 
                    FETCH NEXT @PageSize ROWS ONLY;
                END;
                GO
                
                """;
			migrationBuilder.Sql(sql);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			var sql = "DROP PROCEDURE [dbo].[GetStockItems]";
			migrationBuilder.DropTable(sql);
		}
    }
}
