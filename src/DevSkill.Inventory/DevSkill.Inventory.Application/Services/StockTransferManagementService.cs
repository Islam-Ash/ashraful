using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class StockTransferManagementService : IStockTransferManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public StockTransferManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
        {
            _inventoryUnitOfWork = invertoryUnitOfWork;
        }

        public async Task TransferStockAsync(Guid itemId, Guid sourceWarehouseId, Guid destinationWarehouseId, int transferQuantity, string notes)
        {
            await _inventoryUnitOfWork.StockTransferRepository.GetTransferStockAsync(itemId, sourceWarehouseId, destinationWarehouseId, transferQuantity, notes);
        }

        public async Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetStockTransfersAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.StockTransferRepository.GetPagedStockTransferAsync(pageIndex, pageSize, search, order);
        }

        public async Task DeleteTransferAsync(Guid id)
        {
            await _inventoryUnitOfWork.StockTransferRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }
    }
}
