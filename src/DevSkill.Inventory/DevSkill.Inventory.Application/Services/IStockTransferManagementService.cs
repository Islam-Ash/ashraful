using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IStockTransferManagementService
    {
        Task TransferStockAsync(Guid itemId, Guid sourceWarehouseId,
            Guid destinationWarehouseId, int transferQuantity, string notes);
        Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetStockTransfersAsync(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order);
        Task DeleteTransferAsync(Guid id);
    }
}
