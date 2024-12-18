using DevSkill.Inventory.Application;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public class InventoryUnitOfWork : UnitOfWork,IInventoryUnitOfWork 

    {
        public IProductRepository ProductRepository { get;  private set; }
	    public ICategoryRepository CategoryRepository { get; private set; }
        public IMeasurementRepository MeasurementRepository { get; private set; }
        public ITaxCategoryRepository TaxCategoryRepository { get; private set; }
        public IServiceRepository ServiceRepository { get; private set; }
        public IWarehouseRepository WarehouseRepository { get; private set; }
        public IItemRepository ItemRepository { get; private set; }
        public IStockItemRepository StockItemRepository { get; private set; }
        public IStockTransferRepository StockTransferRepository { get; private set; }
        public IReasonRepository ReasonRepository { get; private set; }


		public InventoryUnitOfWork(InventoryDbContext dbContext,
            IProductRepository productRepository, ICategoryRepository categoryRepository,
            IMeasurementRepository measurementRepository, ITaxCategoryRepository taxCategoryRepository, 
            IServiceRepository serviceRepository, IWarehouseRepository warehouseRepository,
            IItemRepository itemRepository, IStockItemRepository stockItemRepository, 
            IStockTransferRepository stockTransferRepository, IReasonRepository reasonRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            MeasurementRepository = measurementRepository;
            TaxCategoryRepository = taxCategoryRepository;
            ServiceRepository = serviceRepository;
            WarehouseRepository = warehouseRepository;
            ItemRepository = itemRepository;
            StockItemRepository = stockItemRepository;
            StockTransferRepository = stockTransferRepository;
            ReasonRepository = reasonRepository;
        }

        public async Task<(IList<ItemDto> data, int total, int totalDisplay)> GetPagedItemsUsingSPAsync( int pageIndex, 
            int pageSize,ItemSearchDto search,string? order)
        {
            var procedureName = "GetItems";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<ItemDto>(procedureName, // Use ItemDto as the type here
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order ?? "Name" },
                    { "PriceStartFrom", string.IsNullOrEmpty(search.PriceStartFrom) ? (decimal?)null : decimal.Parse(search.PriceStartFrom) },
                    { "PriceStartTo", string.IsNullOrEmpty(search.PriceStartTo) ? (decimal?)null : decimal.Parse(search.PriceStartTo) },
                    { "Name", string.IsNullOrEmpty(search.Name) ? null: search.Name },
                    { "Barcode", string.IsNullOrEmpty(search.Barcode) ? null : search.Barcode },
                    { "CategoryId", string.IsNullOrEmpty(search.CategoryId) ? null : Guid.Parse(search.CategoryId) }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return (
                data: result.result,
                total: (int)result.outValues["Total"],
                totalDisplay: (int)result.outValues["TotalDisplay"]
            );
        }

		public async Task<(IList<StockItemDto> data, int total, int totalDisplay)> GetPagedStockItemsUsingSPAsync(
	        int pageIndex, int pageSize, StockItemSearchDto search, string? order)
		{
			if (search == null)
			{
				throw new ArgumentNullException(nameof(search), "The search parameter cannot be null.");
			}

			var procedureName = "GetStockItems";

			var parameters = new Dictionary<string, object>
	        {
			   { "PageIndex", pageIndex },
					{ "PageSize", pageSize },
					{ "OrderBy", order ?? "ItemName" },
					{ "ItemName", string.IsNullOrEmpty(search.ItemName) ? null: search.ItemName },
					{ "Barcode", string.IsNullOrEmpty(search.Barcode) ? null : search.Barcode },
                    { "WarehouseId", string.IsNullOrEmpty(search.WarehouseId) ? null : Guid.Parse(search.WarehouseId) },
					{ "CategoryId", string.IsNullOrEmpty(search.CategoryId) ? null : Guid.Parse(search.CategoryId) }
			};

			var result = await SqlUtility.QueryWithStoredProcedureAsync<StockItemDto>(
				procedureName, parameters, new Dictionary<string, Type>
				{
			        { "Total", typeof(int) },
			        { "TotalDisplay", typeof(int) }
				});

			return (
				data: result.result,
				total: (int)result.outValues["Total"],
				totalDisplay: (int)result.outValues["TotalDisplay"]
			);
		}



	}
}
