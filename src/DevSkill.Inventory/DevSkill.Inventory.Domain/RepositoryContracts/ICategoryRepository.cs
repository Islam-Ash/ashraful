using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
	public interface ICategoryRepository : IRepositoryBase<Category, Guid>
	{
       Task<(IList<Category> data, int total, int totalDisplay)> GetPagedCategoriesAsync(int pageIndex,
            int pageSize, DataTablesSearch search, string? order);

		Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null);
		//Task<Category> GetCategoryAsync(Guid Id);


	}
}
