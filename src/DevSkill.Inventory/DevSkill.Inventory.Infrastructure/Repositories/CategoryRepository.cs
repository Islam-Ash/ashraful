using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
	public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
	{
		public CategoryRepository(InventoryDbContext context) : base(context)
		{
		}

		//public Task<Category> GetCategoryAsync(Guid Id)
		//{
			
		//}

		public async Task<(IList<Category> data, int total, int totalDisplay)> GetPagedCategoriesAsync(int pageIndex, 
            int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
				return await GetDynamicAsync(null, order, null, pageIndex, pageSize, true);
			else
				return await GetDynamicAsync(x => x.Name == search.Value, order, null, pageIndex, pageSize, true);
		}

		public async Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null)
		{
			if (id.HasValue)
			{
				return await GetCountAsync(x => x.Id != id.Value && x.Name == title) > 0;
			}
			else
			{
				return await GetCountAsync(x => x.Name == title) > 0;
			}
		}
		public bool IsTitleDuplicate(string title, Guid? id = null)
        {

            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.Name == title) > 0;
            }
            else
            {
                return GetCount(x => x.Name == title) > 0;
            }
        }
    }
}
