using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class UpdateGetItemSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetItems 
                   	@PageIndex int,
                   	@PageSize int , 
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

                   	Declare @sql nvarchar(2000);
                   	Declare @countsql nvarchar(2000);
                   	Declare @paramList nvarchar(MAX); 
                   	Declare @countparamList nvarchar(MAX);

                   	-- Collecting Total
                   	Select @Total = count(*) from Items;

                   	-- Collecting Total Display
                   	SET @countsql = 'select @TotalDisplay = count(*) from Items pro inner join 
                               					Categories c on pro.CategoryId = c.Id where 1 = 1 ';

                   	SET @countsql = @countsql + ' AND pro.Name LIKE ''%'' + @xName + ''%''' 

                   	SET @countsql = @countsql + ' AND pro.Barcode LIKE ''%'' + @xBarcode + ''%''' 

                   	IF @PriceStartFrom IS NOT NULL
                   	SET @countsql = @countsql + ' AND pro.SellingPrice >= @xPriceStartFrom'

                   	IF @PriceStartTo IS NOT NULL
                   	SET @countsql = @countsql + ' AND pro.SellingPrice <= @xPriceStartTo' 

                   	IF @CategoryId IS NOT NULL
                   	SET @countsql = @countsql + ' AND pro.CategoryId = @xCategoryId' 

                   	SELECT @countparamlist = '@xName nvarchar(max),
                      		@xBarcode nvarchar(max),
                      		@xPriceStartFrom decimal,
                      		@xPriceStartTo decimal,
                      		@xCategoryId uniqueidentifier,
                      		@TotalDisplay int output' ;

                   	exec sp_executesql @countsql , @countparamlist ,
                      		@Name,
                      		@Barcode,
                      		@PriceStartFrom,
                      		@PriceStartTo,
                      		@CategoryId,
                      		@TotalDisplay = @TotalDisplay output;

                   	-- Collecting Data
                   	SET @sql = 'select pro.Id, pro.Name, pro.Barcode, pro.SellingPrice, c.Name as CategoryName from Items pro inner join 
                               					Categories c on pro.CategoryId = c.Id where 1 = 1 ';

                   	SET @sql = @sql + ' AND pro.Name LIKE ''%'' + @xName + ''%''' 

                   	SET @sql = @sql + ' AND pro.Barcode LIKE ''%'' + @xBarcode + ''%''' 

                   	IF @PriceStartFrom IS NOT NULL
                   	SET @sql = @sql + ' AND pro.SellingPrice >= @xPriceStartFrom'

                   	IF @PriceStartTo IS NOT NULL
                   	SET @sql = @sql + ' AND pro.SellingPrice <= @xPriceStartTo' 

                   	IF @CategoryId IS NOT NULL
                   	SET @sql = @sql + ' AND pro.CategoryId = @xCategoryId' 

                   	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                   	ROWS FETCH NEXT @PageSize ROWS ONLY';

                   	SELECT @paramlist = '@xName nvarchar(max),
                      		@xBarcode nvarchar(max),
                      		@xPriceStartFrom decimal,
                      		@xPriceStartTo decimal,
                      		@xCategoryId uniqueidentifier,
                      		@PageIndex int,
                      		@PageSize int' ;

                   	exec sp_executesql @sql , @paramlist ,
                      		@Name,
                      		@Barcode,
                      		@PriceStartFrom,
                      		@PriceStartTo,
                      		@CategoryId,
                      		@PageIndex,
                      		@PageSize;

                   	print @sql;
                   	print @countsql;

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
