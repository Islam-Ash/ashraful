using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class ServicesManagementService : IServicesManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public ServicesManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
        {
            _inventoryUnitOfWork = invertoryUnitOfWork;
        }

        public async Task CreateServiceAsync(Service service)
        {
            if (!await _inventoryUnitOfWork.ServiceRepository.IsTitleDuplicateAsync(service.Name))
            {
                await _inventoryUnitOfWork.ServiceRepository.AddAsync(service);
               await _inventoryUnitOfWork.SaveAsync();
            }
        }

        public async Task DeleteServiceAsync(Guid id)
        {
           await _inventoryUnitOfWork.ServiceRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task <Service> GetServiceAsync(Guid id)
        {
           return await _inventoryUnitOfWork.ServiceRepository.GetByIdAsync(id);
        }

        public async Task<(IList<Service> data, int total, int totalDisplay)> GetServicesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.ServiceRepository.GetPagedServicesAsync(pageIndex, pageSize, search, order);
        }
        public async Task UpdateServiceAsync(Service service)
        {
            if (!await _inventoryUnitOfWork.ServiceRepository.IsTitleDuplicateAsync(service.Name, service.Id))
            {
                await _inventoryUnitOfWork.ServiceRepository.EditAsync(service);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }

        public decimal CalculateTaxedPrice(bool isInclusive, decimal price, decimal taxPrecentage)
        {
            
            decimal taxRate = taxPrecentage / 100;

            if (isInclusive)
            {
                return Math.Round(price / (1 + taxRate), 2);
            }
            else
            {
                return Math.Round(price * (1 + taxRate), 2);
            }
        }

	}
}
