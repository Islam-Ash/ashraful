using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application
{
    public interface IInventoryUnitOfWork : IUnitOfWork
    {
        public IProductRepository ProductRepository { get;  }
		public ICategoryRepository CategoryRepository { get; }
        public IMeasurementRepository MeasurementRepository { get; }
        public ITaxCategoryRepository TaxCategoryRepository { get; }
        public IServiceRepository ServiceRepository { get; }
        public IWarehouseRepository WarehouseRepository { get; }
        public IItemRepository ItemRepository { get; }
        public IStockItemRepository StockItemRepository { get; }
        public IStockTransferRepository StockTransferRepository { get; }
        public IReasonRepository ReasonRepository { get; }

        Task<(IList<ItemDto> data, int total, int totalDisplay)> GetPagedItemsUsingSPAsync(int pageIndex,
            int pageSize, ItemSearchDto search, string? order);
		Task<(IList<StockItemDto> data, int total, int totalDisplay)> GetPagedStockItemsUsingSPAsync(int pageIndex,
		   int pageSize, StockItemSearchDto search, string? order);

	}
}
