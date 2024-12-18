using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IStockTransferRepository: IRepositoryBase<StockTransfer, Guid>
    {
        bool IsTitleDuplicate(string title, Guid? id = null);
        Task GetTransferStockAsync(Guid itemId, Guid sourceWarehouseId, Guid destinationWarehouseId, int transferQuantity, string notes);
        Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetPagedStockTransferAsync(int pageIndex,
           int pageSize, DataTablesSearch search, string? order);
        //Task DeleteAsync(Guid id);
    }
}
