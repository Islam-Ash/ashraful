
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
    public interface IStockItemManagementService
    {
		Task<(IList<StockItemDto> data, int total, int totalDisplay)> GetStockItemsSPAsync(int pageIndex,
			int pageSize, StockItemSearchDto search, string? order);
		Task<(IList<StockAdjustment> data, int total, int totalDisplay)> GetStockAdjustmentsAsync(int pageIndex, int pageSize,
           DataTablesSearch search, string? order);

        Task<IList<StockItem>> GetStockItemsAsyncByWarehouse(Guid warehouseId);
        Task CreateforUpdateAsync(StockAdjustment stockAdjustment);
		void DeleteAdjustment(Guid id);

	}
}
