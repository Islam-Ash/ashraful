using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ItemRepository : Repository<Item, Guid>, IItemRepository
    {
		private readonly InventoryDbContext _context;
        public ItemRepository(InventoryDbContext context) : base(context)
        {
			_context = context;
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
  
		public async Task<Item> GetItemAsync(Guid id)
        {
            return (await GetAsync(x => x.Id == id, y => y.Include(z => z.Category))).FirstOrDefault();
        }

    }
}
