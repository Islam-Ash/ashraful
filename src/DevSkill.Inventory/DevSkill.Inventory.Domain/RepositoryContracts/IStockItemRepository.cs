using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockItemRepository :IRepositoryBase<StockItem, Guid>
    {
        Task<(IList<StockAdjustment> data, int total, int totalDisplay)> GetPagedStockAdjustmentsAsync(int pageIndex,
           int pageSize, DataTablesSearch search, string? order);

        bool IsTitleDuplicate(string title, Guid? id = null);
        Task<IList<StockItem>> GetWarehouseStockItemAsync(Guid warehouseId);
        Task CreateAsync(StockAdjustment stockAdjustment);
        void RemoveAdjustment(Guid id);

	}
}
