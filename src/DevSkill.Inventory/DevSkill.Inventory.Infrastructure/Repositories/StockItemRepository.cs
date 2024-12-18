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
    public class StockItemRepository : Repository<StockItem, Guid>, IStockItemRepository
    {
		private readonly InventoryDbContext _context;
		public StockItemRepository(InventoryDbContext context) : base(context)
        {
			_context = context;
        }

        public async Task<(IList<StockAdjustment> data, int total, int totalDisplay)> GetPagedStockAdjustmentsAsync(
             int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            var query = _context.StockAdjustments
                                .Include(x => x.Item)
                                .Include(x => x.Warehouse)
                                .Include(x => x.Reason)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search.Value))
            {
                query = query.Where(x => x.Item.Name.Contains(search.Value) ||
                                         x.Warehouse.Name.Contains(search.Value) ||
                                         x.Reason.Name.Contains(search.Value));
            }

            // Ordering logic
            query = order switch
            {
                "Item" => query.OrderBy(x => x.Item.Name),
                "Warehouse" => query.OrderBy(x => x.Warehouse.Name),
                "Quantity" => query.OrderBy(x => x.Quantity),
                "Reason" => query.OrderBy(x => x.Reason.Name),
                _ => query.OrderBy(x => x.Id), 
            };

            int total = await query.CountAsync();

            var pagedQuery = query.Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize);

            int totalDisplay = await pagedQuery.CountAsync();

            var data = await pagedQuery.ToListAsync();

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
		public async Task<IList<StockItem>> GetWarehouseStockItemAsync(Guid warehouseId)
		{
            return await _context.StockItems
                .Where(si => si.WarehouseId == warehouseId)
                .Include(si => si.Item)
                .ToListAsync();
        }

        public async Task CreateAsync(StockAdjustment stockAdjustment)
        {
            if (stockAdjustment == null)
                throw new ArgumentNullException(nameof(stockAdjustment), "Stock adjustment cannot be null.");

            var stockItem = await _context.StockItems
                .FirstOrDefaultAsync(si => si.ItemId == stockAdjustment.ItemId && si.WarehouseId == stockAdjustment.WarehouseId);

            if (stockItem == null)
                throw new Exception("Stock item not found for the specified item and warehouse.");

            stockItem.Quantity += stockAdjustment.Quantity;

            if (stockItem.Quantity < 0)
                throw new InvalidOperationException("Stock quantity cannot be negative.");

            stockItem.AsOfDate = DateTime.UtcNow;

            _context.StockAdjustments.Add(stockAdjustment);
            await _context.SaveChangesAsync();
        }
        
		public void RemoveAdjustment(Guid id)
		{
			var adjustment = _context.StockAdjustments.FirstOrDefault(sa => sa.Id == id);

			if (adjustment == null)
			{
				throw new InvalidOperationException("Adjustment not found.");
			}

			_context.StockAdjustments.Remove(adjustment);
			_context.SaveChanges();
		}


	}
}
