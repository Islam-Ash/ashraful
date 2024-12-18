using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
	public class WarehouseManagementService : IWarehouseManagementService
	{
		private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

		public WarehouseManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
		{
			_inventoryUnitOfWork = invertoryUnitOfWork;
		}

		public async Task CreateWarehouseAsync(Warehouse warehouse)
		{
			if (!await _inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicateAsync(warehouse.Name))
			{
				await _inventoryUnitOfWork.WarehouseRepository.AddAsync(warehouse);
				await _inventoryUnitOfWork.SaveAsync();
			}
		}

		public async Task DeleteWarehouseAsync(Guid id)
		{
			await _inventoryUnitOfWork.WarehouseRepository.RemoveAsync(id);
			await _inventoryUnitOfWork.SaveAsync();
		}

		public async Task<IList<Warehouse>> GetWarehousesAsync()
		{
			return await _inventoryUnitOfWork.WarehouseRepository.GetAllAsync();
		}

		public async Task< (IList<Warehouse> data, int total, int totalDisplay)> GetWarehousesAsync(int pageIndex,
			int pageSize, DataTablesSearch search, string? order)
		{
			return await _inventoryUnitOfWork.WarehouseRepository.GetPagedWarehousesAsync(pageIndex, pageSize, search, order);
		}

		public async Task<Warehouse> GetWarehouseAsync(Guid warehouseId)
		{
			return await _inventoryUnitOfWork.WarehouseRepository.GetByIdAsync(warehouseId);
		}

		public async Task UpdateWarehouseAsync(Warehouse warehouse)
		{
			if (!await _inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicateAsync(warehouse.Name))
			{
				await _inventoryUnitOfWork.WarehouseRepository.EditAsync(warehouse);
				await _inventoryUnitOfWork.SaveAsync();
			}
		}

	}
}
