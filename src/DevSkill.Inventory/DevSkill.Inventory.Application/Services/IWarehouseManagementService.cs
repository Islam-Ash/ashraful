using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Application.Services
{
	public interface IWarehouseManagementService
	{
		Task <IList<Warehouse>> GetWarehousesAsync();
		Task<Warehouse> GetWarehouseAsync(Guid warehouseId);
		Task CreateWarehouseAsync(Warehouse warehouse);
		Task<(IList<Warehouse> data, int total, int totalDisplay)> GetWarehousesAsync(int pageIndex, int pageSize,
			DataTablesSearch search, string? order);
		Task UpdateWarehouseAsync(Warehouse warehouse);
		Task DeleteWarehouseAsync(Guid id);
	}
}