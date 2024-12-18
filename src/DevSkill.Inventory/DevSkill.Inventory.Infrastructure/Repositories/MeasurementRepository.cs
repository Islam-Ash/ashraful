using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
	public class MeasurementRepository : Repository<Measurement, Guid>, IMeasurementRepository
	{
		public MeasurementRepository(InventoryDbContext context) : base(context)
		{
		}

		public async Task<(IList<Measurement> data, int total, int totalDisplay)> GetPagedMeasurementUnitsAsync(int pageIndex,
		  int pageSize, DataTablesSearch search, string? order)
		{
            if (string.IsNullOrWhiteSpace(search.Value))
                return await GetDynamicAsync(null, order, null, pageIndex, pageSize, true);
            else
                return await GetDynamicAsync(x => x.MeasurementName == search.Value, order, null, pageIndex, pageSize, true);
        }

		public async Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null)
		{

			if (id.HasValue)
			{
				return await GetCountAsync(x => x.Id != id.Value && x.MeasurementName == title) > 0;
			}
			else
			{
				return await GetCountAsync(x => x.MeasurementName == title) > 0;
			}
		}
	}
}
