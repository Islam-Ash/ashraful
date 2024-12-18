using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Application.Services
{
    public interface IItemManagementService
    {
        Task CreateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
        Task<Item> GetItemAsync(Guid id);
        Task<IList<Item>> GetItemsAsync();
        Task<(IList<ItemDto> data, int total, int totalDisplay)> GetItemsSPAsync(int pageIndex,
            int pageSize, ItemSearchDto search, string? order);
        Task UpdateItemAsync(Item item);
    }
}
