using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
	public interface IMeasurementManagementService
	{
		Task<IList<Measurement>> GetMeasurementUnitsAsync();
		Task<Measurement> GetMeasurementUnitAsync(Guid measurementId);
		Task CreateMeasurementUnitAsync(Measurement measurement);
		Task DeleteMeasurementUnitAsync(Guid id);
		Task<(IList<Measurement> data, int total, int totalDisplay)> GetMeasurementUnitsAsync(int pageIndex, int pageSize,
				DataTablesSearch search, string? order);
		Task UpdateMeasurmentUnitAsync(Measurement measurement);
	

	}
}
