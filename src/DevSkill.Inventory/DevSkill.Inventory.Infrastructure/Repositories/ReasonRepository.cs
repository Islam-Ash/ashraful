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
    public class ReasonRepository : Repository<Reason, Guid>, IReasonRepository
    {
        public ReasonRepository(InventoryDbContext context) : base(context)
        {
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
    }
}
