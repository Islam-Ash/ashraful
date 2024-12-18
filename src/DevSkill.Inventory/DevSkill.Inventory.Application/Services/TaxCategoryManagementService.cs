using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
	public class TaxCategoryManagementService : ITaxCategoryManagementService
	{
		private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

		public TaxCategoryManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
		{
			_inventoryUnitOfWork = invertoryUnitOfWork;
		}

        public async Task<(IList<TaxCategory> data, int total, int totalDisplay)> GetTaxCategoriesAsync(int pageIndex, 
			int pageSize, DataTablesSearch search, string? order )
        {
            return await _inventoryUnitOfWork.TaxCategoryRepository.GetPagedTaxCategoriesAsync(pageIndex, pageSize, search, order);
        }

        public async Task<IList<TaxCategory>> GetTaxCategoriesAsync()
        {
            return await _inventoryUnitOfWork.TaxCategoryRepository.GetAllAsync();
        }

        public async Task<TaxCategory> GetTaxCategoryAsync(Guid TaxCategoryId)
        {
            return await _inventoryUnitOfWork.TaxCategoryRepository.GetByIdAsync(TaxCategoryId);
        }

		
	}
}
