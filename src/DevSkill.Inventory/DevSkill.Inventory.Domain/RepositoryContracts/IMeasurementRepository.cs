using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
	public interface IMeasurementRepository: IRepositoryBase<Measurement, Guid>
	{
		Task<(IList<Measurement> data, int total, int totalDisplay)> GetPagedMeasurementUnitsAsync(int pageIndex,
		  int pageSize, DataTablesSearch search, string? order);

		Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null);
	}
}
