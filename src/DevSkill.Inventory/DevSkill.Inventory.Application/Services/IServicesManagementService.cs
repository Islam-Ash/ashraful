using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IServicesManagementService
    {
        Task<Service> GetServiceAsync(Guid id);
        Task CreateServiceAsync(Service service);
        Task DeleteServiceAsync(Guid id);
        Task<(IList<Service> data, int total, int totalDisplay)> GetServicesAsync(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        Task UpdateServiceAsync(Service service);
        decimal CalculateTaxedPrice(bool isInclusive, decimal price, decimal taxPrecentage);


    }
}
