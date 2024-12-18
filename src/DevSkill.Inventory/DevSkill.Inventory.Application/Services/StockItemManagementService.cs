using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Services
{
    public class StockItemManagementService: IStockItemManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

		public StockItemManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
		{
			_inventoryUnitOfWork = inventoryUnitOfWork;
		
		}
		
        public async Task<(IList<StockAdjustment> data, int total, int totalDisplay)> GetStockAdjustmentsAsync(int pageIndex,
           int pageSize, DataTablesSearch search, string? order)
        {

            return await _inventoryUnitOfWork.StockItemRepository
                .GetPagedStockAdjustmentsAsync(pageIndex, pageSize, search, order);
        }
        public async Task<IList<StockItem>> GetStockItemsAsyncByWarehouse(Guid warehouseId)
        {
            return await _inventoryUnitOfWork.StockItemRepository
                .GetWarehouseStockItemAsync(warehouseId);
        }

        public async Task CreateforUpdateAsync(StockAdjustment stockAdjustment)
        {
            await _inventoryUnitOfWork.StockItemRepository.CreateAsync(stockAdjustment);
        }

		public void DeleteAdjustment(Guid id)
		{
            _inventoryUnitOfWork.StockItemRepository.RemoveAdjustment(id);
            _inventoryUnitOfWork.Save();
		}

		public async Task<(IList<StockItemDto> data, int total, int totalDisplay)> GetStockItemsSPAsync(int pageIndex, int pageSize, StockItemSearchDto search, string? order)
		{
			return await _inventoryUnitOfWork.GetPagedStockItemsUsingSPAsync(pageIndex, pageSize, search, order);
		}

    }
}
