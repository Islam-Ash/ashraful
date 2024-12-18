using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.RepositoryContracts;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
	public class WarehouseRepository : Repository<Warehouse, Guid>, IWarehouseRepository
	{
		public WarehouseRepository(InventoryDbContext context) : base(context)
		{
		}
		public async Task<(IList<Warehouse> data, int total, int totalDisplay)> GetPagedWarehousesAsync(int pageIndex,
			int pageSize, DataTablesSearch search, string? order)
		{
			if (string.IsNullOrWhiteSpace(search.Value))
				return await GetDynamicAsync(null, order, null, pageIndex, pageSize, true);
			else
				return await GetDynamicAsync(x => x.Name == search.Value, order, null, pageIndex, pageSize, true);
		}

		public async Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null)
		{
			if (id.HasValue)
			{
				return await GetCountAsync(x => x.Id != id.Value && x.Name == title) > 0;
			}
			else
			{
				return await GetCountAsync(x => x.Name == title) > 0;
			}
		}
	}
	
}
