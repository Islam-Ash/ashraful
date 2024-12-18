using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class UpdateGetItemsSP3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetItems 
                    @PageIndex int,
                    @PageSize int, 
                    @OrderBy nvarchar(50),
                    @Name nvarchar(max) = '%',
                    @PriceStartFrom decimal = NULL,
                    @PriceStartTo decimal = NULL,
                    @Barcode nvarchar(max) = '%',
                    @CategoryId uniqueidentifier = NULL,
                    @Total int output,
                    @TotalDisplay int output
                AS
                BEGIN
                    SET NOCOUNT ON;
                
                    DECLARE @sql nvarchar(MAX);
                    DECLARE @countsql nvarchar(MAX);
                    DECLARE @paramList nvarchar(MAX); 
                    DECLARE @countparamList nvarchar(MAX);
                
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
                
                    -- Filtering Conditions
                    SET @countsql = @countsql + ' AND pro.Name LIKE ''%'' + @xName + ''%'''
                    SET @countsql = @countsql + ' AND pro.Barcode LIKE ''%'' + @xBarcode + ''%'''
                
                    IF @PriceStartFrom IS NOT NULL
                        SET @countsql = @countsql + ' AND pro.SellingPrice >= @xPriceStartFrom'
                
                    IF @PriceStartTo IS NOT NULL
                        SET @countsql = @countsql + ' AND pro.SellingPrice <= @xPriceStartTo'
                
                    IF @CategoryId IS NOT NULL
                        SET @countsql = @countsql + ' AND pro.CategoryId = @xCategoryId'
                
                    -- Count Parameter List
                    SELECT @countparamlist = '@xName nvarchar(max),
                                              @xBarcode nvarchar(max),
                                              @xPriceStartFrom decimal,
                                              @xPriceStartTo decimal,
                                              @xCategoryId uniqueidentifier,
                                              @TotalDisplay int output';
                
                    EXEC sp_executesql @countsql, @countparamlist,
                        @Name,
                        @Barcode,
                        @PriceStartFrom,
                        @PriceStartTo,
                        @CategoryId,
                        @TotalDisplay = @TotalDisplay OUTPUT;
                
                    -- Collecting Data
                    SET @sql = '
                        SELECT pro.Id, pro.Name, pro.Barcode, pro.SellingPrice, 
                               c.Name as CategoryName, pro.PictureUrl, 
                               stc.Percentage as SellingTaxPercentage, 
                               ISNULL(SUM(si.Quantity), 0) as Quantity, 
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
                
                    -- Adding Ordering and Pagination
                    SET @sql = @sql + ' GROUP BY pro.Id, pro.Name, pro.Barcode, pro.SellingPrice, 
                                               c.Name, pro.PictureUrl, stc.Percentage, pro.IsActive 
                                         ORDER BY ' + @OrderBy + ' 
                                         OFFSET @PageSize * (@PageIndex - 1) ROWS 
                                         FETCH NEXT @PageSize ROWS ONLY';
                
                    -- Data Parameter List
                    SELECT @paramList = '@xName nvarchar(max),
                                         @xBarcode nvarchar(max),
                                         @xPriceStartFrom decimal,
                                         @xPriceStartTo decimal,
                                         @xCategoryId uniqueidentifier,
                                         @PageIndex int,
                                         @PageSize int';
                
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
