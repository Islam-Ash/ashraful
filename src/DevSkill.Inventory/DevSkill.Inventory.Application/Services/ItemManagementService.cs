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
    public class ItemManagementService : IItemManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public ItemManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task CreateItemAsync(Item item)
        {
            if (!await _inventoryUnitOfWork.ItemRepository.IsTitleDuplicateAsync(item.Name))
            {
                await _inventoryUnitOfWork.ItemRepository.AddAsync(item);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _inventoryUnitOfWork.ItemRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            return await _inventoryUnitOfWork.ItemRepository.GetItemAsync(id);
        }
        public async Task<(IList<ItemDto> data, int total, int totalDisplay)> GetItemsSPAsync(int pageIndex, int pageSize, ItemSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.GetPagedItemsUsingSPAsync(pageIndex, pageSize, search, order);
        }

        public async Task UpdateItemAsync(Item item)
        {
            if (!await _inventoryUnitOfWork.ItemRepository.IsTitleDuplicateAsync(item.Name, item.Id))
            {
                await _inventoryUnitOfWork.ItemRepository.EditAsync(item);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
                throw new InvalidOperationException("Title Should be Unique");

        }
        public async Task<IList<Item>> GetItemsAsync()
        {
            return await _inventoryUnitOfWork.ItemRepository.GetAllAsync();
        }
    }
}
