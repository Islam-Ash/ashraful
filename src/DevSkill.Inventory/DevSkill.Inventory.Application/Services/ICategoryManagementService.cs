using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
	public interface ICategoryManagementService
	{
        IList<Category> GetCategories();
        Task<IList<Category>> GetCategoriesAsync();
        Task <Category> GetCategoryAsync(Guid categoryId);
		Task CreateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Guid id);
		Task<(IList<Category> data, int total, int totalDisplay)> GetCategoriesAsync(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        Task UpdateCategoryAsync(Category category);
    }
}
