using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface ITaxCategoryManagementService
    {
		Task<TaxCategory> GetTaxCategoryAsync(Guid TaxCategoryId);
		Task<IList<TaxCategory>> GetTaxCategoriesAsync();
		Task<(IList<TaxCategory> data, int total, int totalDisplay)>GetTaxCategoriesAsync(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order);
    }
}
