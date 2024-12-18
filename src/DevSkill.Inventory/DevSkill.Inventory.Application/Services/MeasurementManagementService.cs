using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
	public class MeasurementManagementService : IMeasurementManagementService
	{
		private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

		public MeasurementManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
		{
			_inventoryUnitOfWork = invertoryUnitOfWork;
		}

		public async Task CreateMeasurementUnitAsync(Measurement measurement)
		{
			if (!await _inventoryUnitOfWork.MeasurementRepository.IsTitleDuplicateAsync(measurement.MeasurementName))
			{
				await _inventoryUnitOfWork.MeasurementRepository.AddAsync(measurement);
				await _inventoryUnitOfWork.SaveAsync();
			}
		}
		public async Task UpdateMeasurmentUnitAsync(Measurement measurement)
		{
			if (!await _inventoryUnitOfWork.MeasurementRepository.IsTitleDuplicateAsync(measurement.MeasurementName, measurement.Id))
			{
				await _inventoryUnitOfWork.MeasurementRepository.EditAsync(measurement);
				await _inventoryUnitOfWork.SaveAsync();
			}
			else
				throw new InvalidOperationException("Title Should be Unique");
		}
		public async Task DeleteMeasurementUnitAsync(Guid id)
		{
			await _inventoryUnitOfWork.MeasurementRepository.RemoveAsync(id);
			 await _inventoryUnitOfWork.SaveAsync();
		}

		public async Task <Measurement> GetMeasurementUnitAsync(Guid measurementId)
		{
			return await _inventoryUnitOfWork.MeasurementRepository.GetByIdAsync(measurementId);
		}

		public async Task<IList<Measurement>> GetMeasurementUnitsAsync()
		{
			return await _inventoryUnitOfWork.MeasurementRepository.GetAllAsync();
		}

		public async Task<(IList<Measurement> data, int total, int totalDisplay)> GetMeasurementUnitsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
		{
			return await _inventoryUnitOfWork.MeasurementRepository.GetPagedMeasurementUnitsAsync(pageIndex, pageSize, search, order);
		}

		
	}
}
