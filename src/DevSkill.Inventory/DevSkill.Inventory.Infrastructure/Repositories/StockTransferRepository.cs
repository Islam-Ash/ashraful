using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class StockTransferRepository : Repository<StockTransfer, Guid>, IStockTransferRepository
    {
        private readonly InventoryDbContext _context;
        public StockTransferRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(IList<StockTransfer> data, int total, int totalDisplay)> GetPagedStockTransferAsync(
      int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            Expression<Func<StockTransfer, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(search.Value))
            {
                filter = x => x.Item.Name.Contains(search.Value);
            }

            var total = await _context.StockTransfers.CountAsync();

            var totalDisplay = filter != null
                ? await _context.StockTransfers.CountAsync(filter)
                : total;

            var query = _context.StockTransfers
                .Include(st => st.Item)
                .Include(st => st.SourceWarehouse)
                .Include(st => st.DestinationWarehouse)
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

              if (!string.IsNullOrWhiteSpace(order))
            {
                query = query.OrderBy(order); 
            }

            var data = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total, totalDisplay);
        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.Item.Name == title) > 0;
            }
            else
            {
                return GetCount(x => x.Item.Name == title) > 0;
            }
        }
        public async Task GetTransferStockAsync(Guid itemId, Guid sourceWarehouseId, Guid destinationWarehouseId, int transferQuantity, string notes)
        {

            if (sourceWarehouseId == destinationWarehouseId)
            {
                throw new ArgumentException("Source and destination warehouses cannot be the same.");
            }

            var sourceStockItem = await _context.StockItems
                .FirstOrDefaultAsync(si => si.ItemId == itemId && si.WarehouseId == sourceWarehouseId);

            var destinationStockItem = await _context.StockItems
                .FirstOrDefaultAsync(si => si.ItemId == itemId && si.WarehouseId == destinationWarehouseId);

            if (sourceStockItem == null || sourceStockItem.Quantity < transferQuantity)
            {
                throw new InvalidOperationException("Insufficient stock in source warehouse for this transfer.");
            }

            sourceStockItem.Quantity -= transferQuantity;

            if (destinationStockItem != null)
            {
                destinationStockItem.Quantity += transferQuantity;
            }
            else
            {
                destinationStockItem = new StockItem
                {
                    ItemId = itemId,
                    WarehouseId = destinationWarehouseId,
                    Quantity = transferQuantity
                };
                _context.StockItems.Add(destinationStockItem);
            }

            var transferRecord = new StockTransfer
            {
                ItemId = itemId,
                SourceWarehouseId = sourceWarehouseId,
                DestinationWarehouseId = destinationWarehouseId,
                Quantity = transferQuantity,
                TransferDate = DateTime.Now,
                Note = notes
            };
            _context.StockTransfers.Add(transferRecord);
            await _context.SaveChangesAsync();
        }

    }
}
