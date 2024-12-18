﻿using DevSkill.Inventory.Domain;
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
	public class TaxCategoryRepository : Repository<TaxCategory, Guid>, ITaxCategoryRepository
	{
		public TaxCategoryRepository(InventoryDbContext context) : base(context)
		{
		}

        public async Task<(IList<TaxCategory> data, int total, int totalDisplay)> GetPagedTaxCategoriesAsync(int pageIndex,
            int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return await GetDynamicAsync(null, order, null, pageIndex, pageSize, true);
            else
                return await GetDynamicAsync(x => x.Name == search.Value, order, null, pageIndex, pageSize, true);
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