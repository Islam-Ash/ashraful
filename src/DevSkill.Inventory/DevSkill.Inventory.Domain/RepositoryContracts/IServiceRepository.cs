using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IServiceRepository : IRepositoryBase<Service, Guid>
    {
        Task<(IList<Service> data, int total, int totalDisplay)> GetPagedServicesAsync(int pageIndex,
            int pageSize, DataTablesSearch search, string? order);
       
        bool IsTitleDuplicate(string title, Guid? id = null);
		Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null);
		Task<Service> GetServiceAsync(Guid id);

    }
}
