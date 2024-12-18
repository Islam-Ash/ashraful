using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class UpdateGetItemsSP4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                CREATE OR ALTER PROCEDURE GetItems 
                     @PageIndex INT,
                     @PageSize INT, 
                     @OrderBy NVARCHAR(50),
                     @Name NVARCHAR(MAX) = '%',
                     @PriceStartFrom DECIMAL = NULL,
                     @PriceStartTo DECIMAL = NULL,
                     @Barcode NVARCHAR(MAX) = '%',
                     @CategoryId UNIQUEIDENTIFIER = NULL,
                     @Total INT OUTPUT,
                     @TotalDisplay INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @sql NVARCHAR(MAX);
                    DECLARE @countsql NVARCHAR(MAX);
                    DECLARE @paramList NVARCHAR(MAX); 
                    DECLARE @countparamList NVARCHAR(MAX);

                    -- Collecting Total
                    SELECT @Total = COUNT(*) FROM Items;

                    -- Collecting Total Display
                    SET @countsql = '
                        SELECT @TotalDisplay = COUNT(*)
                        FROM Items pro 
                        INNER JOIN Categories c ON pro.CategoryId = c.Id
                        LEFT JOIN TaxCategories stc ON pro.SellingTaxCategoryId = stc.Id
                        LEFT JOIN StockItems si ON pro.Id = si.ItemId
                        WHERE 1 = 1';

                    -- Filtering Conditions for Count
                    SET @countsql = @countsql + ' AND pro.Name LIKE ''%'' + @xName + ''%'''
                    SET @countsql = @countsql + ' AND pro.Barcode LIKE ''%'' + @xBarcode + ''%'''

                    IF @PriceStartFrom IS NOT NULL
                        SET @countsql = @countsql + ' AND pro.SellingPrice >= @xPriceStartFrom'

                    IF @PriceStartTo IS NOT NULL
                        SET @countsql = @countsql + ' AND pro.SellingPrice <= @xPriceStartTo'

                    IF @CategoryId IS NOT NULL
                        SET @countsql = @countsql + ' AND pro.CategoryId = @xCategoryId'

                    -- Count Parameter List
                    SELECT @countparamList = '@xName NVARCHAR(MAX),
                                              @xBarcode NVARCHAR(MAX),
                                              @xPriceStartFrom DECIMAL,
                                              @xPriceStartTo DECIMAL,
                                              @xCategoryId UNIQUEIDENTIFIER,
                                              @TotalDisplay INT OUTPUT';

                    EXEC sp_executesql @countsql, @countparamList,
                        @Name,
                        @Barcode,
                        @PriceStartFrom,
                        @PriceStartTo,
                        @CategoryId,
                        @TotalDisplay = @TotalDisplay OUTPUT;

                    -- Collecting Data with Grouping, Filtering, and Dynamic Ordering
                    SET @sql = '
                        SELECT pro.Id, pro.Name, pro.Barcode, pro.SellingPrice, 
                               c.Name AS CategoryName, pro.PictureUrl, 
                               stc.Percentage AS SellingTaxPercentage, 
                               ISNULL(SUM(si.Quantity), 0) AS Quantity, 
                               pro.IsActive
                        FROM Items pro 
                        INNER JOIN Categories c ON pro.CategoryId = c.Id
                        LEFT JOIN TaxCategories stc ON pro.SellingTaxCategoryId = stc.Id
                        LEFT JOIN StockItems si ON pro.Id = si.ItemId
                        WHERE 1 = 1';

                    -- Filtering Conditions
                    SET @sql = @sql + ' AND pro.Name LIKE ''%'' + @xName + ''%'''
                    SET @sql = @sql + ' AND pro.Barcode LIKE ''%'' + @xBarcode + ''%'''

                    IF @PriceStartFrom IS NOT NULL
                        SET @sql = @sql + ' AND pro.SellingPrice >= @xPriceStartFrom'

                    IF @PriceStartTo IS NOT NULL
                        SET @sql = @sql + ' AND pro.SellingPrice <= @xPriceStartTo'

                    IF @CategoryId IS NOT NULL
                        SET @sql = @sql + ' AND pro.CategoryId = @xCategoryId'

                    -- Adding Grouping
                    SET @sql = @sql + ' GROUP BY pro.Id, pro.Name, pro.Barcode, pro.SellingPrice, 
                                               c.Name, pro.PictureUrl, stc.Percentage, pro.IsActive'

                    -- Adding Dynamic Ordering
                    SET @sql = @sql + ' ORDER BY ' +
                        CASE 
                            WHEN @OrderBy = 'Name' THEN 'pro.Name'
                            WHEN @OrderBy = 'Barcode' THEN 'pro.Barcode'
                            WHEN @OrderBy = 'CategoryName' THEN 'c.Name'
                            WHEN @OrderBy = 'SellingTaxPercentage' THEN 'stc.Percentage'
                            WHEN @OrderBy = 'Quantity' THEN 'ISNULL(SUM(si.Quantity), 0)'
                            WHEN @OrderBy = 'IsActive' THEN 'pro.IsActive'
                            ELSE 'pro.Name'  -- Default ordering
                        END

                    -- Adding Pagination
                    SET @sql = @sql + ' OFFSET @PageSize * (@PageIndex - 1) ROWS 
                                          FETCH NEXT @PageSize ROWS ONLY';

                    -- Data Parameter List
                    SELECT @paramList = '@xName NVARCHAR(MAX),
                                         @xBarcode NVARCHAR(MAX),
                                         @xPriceStartFrom DECIMAL,
                                         @xPriceStartTo DECIMAL,
                                         @xCategoryId UNIQUEIDENTIFIER,
                                         @PageIndex INT,
                                         @PageSize INT';

                    EXEC sp_executesql @sql, @paramList,
                        @Name,
                        @Barcode,
                        @PriceStartFrom,
                        @PriceStartTo,
                        @CategoryId,
                        @PageIndex,
                        @PageSize;

                    PRINT @sql;
                    PRINT @countsql;
                END
                GO
                
                """;
            migrationBuilder.Sql(sql);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetItems]";
            migrationBuilder.DropTable(sql);

        }
    }
}
