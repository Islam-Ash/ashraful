using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
	public class CategoryManagementService : ICategoryManagementService
	{
		private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

		public CategoryManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
		{
			_inventoryUnitOfWork = invertoryUnitOfWork;
		}

        public async Task CreateCategoryAsync(Category category)
        {
            if (!await _inventoryUnitOfWork.CategoryRepository.IsTitleDuplicateAsync(category.Name))
            {
                await _inventoryUnitOfWork.CategoryRepository.AddAsync(category);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }

		public async Task DeleteCategoryAsync(Guid id)
		{
			await _inventoryUnitOfWork.CategoryRepository.RemoveAsync(id);
			await _inventoryUnitOfWork.SaveAsync();
		}

		public async Task<IList<Category>> GetCategoriesAsync() 
		{
			return await _inventoryUnitOfWork.CategoryRepository.GetAllAsync();
		}
        public IList<Category> GetCategories()
        {
            return _inventoryUnitOfWork.CategoryRepository.GetAll();
        }
        public async Task<(IList<Category> data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order)
        {
           return await _inventoryUnitOfWork.CategoryRepository.GetPagedCategoriesAsync(pageIndex, pageSize, search, order);
        }

		public async Task<Category> GetCategoryAsync(Guid categoryId)
        {
            return await _inventoryUnitOfWork.CategoryRepository.GetByIdAsync(categoryId);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if(!await _inventoryUnitOfWork.CategoryRepository.IsTitleDuplicateAsync(category.Name, category.Id))
            {
				await _inventoryUnitOfWork.CategoryRepository.EditAsync(category);
				await _inventoryUnitOfWork.SaveAsync();
            }
        }
    }
}
