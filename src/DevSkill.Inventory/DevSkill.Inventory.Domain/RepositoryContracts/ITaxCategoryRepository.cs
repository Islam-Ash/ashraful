using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface ITaxCategoryRepository : IRepositoryBase<TaxCategory, Guid>
    {
        Task<(IList<TaxCategory> data, int total, int totalDisplay)> GetPagedTaxCategoriesAsync(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order);
    }
}
