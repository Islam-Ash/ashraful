using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ServiceRepository : Repository<Service, Guid>, IServiceRepository
    {
        private readonly InventoryDbContext _context;
        public ServiceRepository(InventoryDbContext context) : base(context)
        {
            _context = context;
        }

		public async Task<(IList<Service> data, int total, int totalDisplay)> GetPagedServicesAsync(
			int pageIndex, int pageSize, DataTablesSearch search, string? order)
		{
			// Construct the query and include TaxCategory
			var query = _context.Services.Include(s => s.TaxCategory).AsQueryable();

			// Apply search filter if there's any search value
			if (!string.IsNullOrWhiteSpace(search.Value))
			{
				query = query.Where(x => x.Name.Contains(search.Value) || x.TaxCategory.Name.Contains(search.Value));
			}

			// Apply sorting if there is an order parameter
			if (!string.IsNullOrWhiteSpace(order))
			{
				query = ApplySorting(query, order);
			}

			// Total count of services
			int total = await query.CountAsync();

			// Pagination
			var pagedQuery = await query.Skip((pageIndex - 1) * pageSize)
										.Take(pageSize)
										.ToListAsync();

			return (pagedQuery, total, pagedQuery.Count);
		}

		private IQueryable<Service> ApplySorting(IQueryable<Service> query, string order)
		{
			// Parse the 'order' value (usually in the format of "column_name direction")
			var orderParts = order.Split(' ');
			var columnName = orderParts[0];
			var direction = orderParts.Length > 1 && orderParts[1].ToLower() == "desc" ? "desc" : "asc";

			// Apply sorting based on the column name and direction
			switch (columnName.ToLower())
			{
				case "name":
					query = direction == "asc" ? query.OrderBy(s => s.Name) : query.OrderByDescending(s => s.Name);
					break;
				case "taxcategory":
					query = direction == "asc" ? query.OrderBy(s => s.TaxCategory.Percentage) : query.OrderByDescending(s => s.TaxCategory.Percentage);
					break;
				case "price":
					query = direction == "asc" ? query.OrderBy(s => s.SellingPriceTaxed) : query.OrderByDescending(s => s.SellingPriceTaxed);
					break;
				
				default:
					query = query.OrderBy(s => s.Name); // Default sorting by Name
					break;
			}

			return query;
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

        public async Task<Service> GetServiceAsync(Guid id)
        {
            return (await GetAsync(x => x.Id == id, y => y.Include(z => z.TaxCategory))).FirstOrDefault();
        }

    }
}
