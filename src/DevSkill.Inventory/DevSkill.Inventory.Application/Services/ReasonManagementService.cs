using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class ReasonManagementService : IReasonManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public ReasonManagementService(IInventoryUnitOfWork invertoryUnitOfWork)
        {
            _inventoryUnitOfWork = invertoryUnitOfWork;
        }

        public async Task CreateReasonAsync(Reason reason)
        {
            if (!await _inventoryUnitOfWork.ReasonRepository.IsTitleDuplicateAsync(reason.Name))
            {
                await _inventoryUnitOfWork.ReasonRepository.AddAsync(reason);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }
        public async Task<IList<Reason>> GetReasonsAsync()
        {
            return await _inventoryUnitOfWork.ReasonRepository.GetAllAsync();
        }
    }
}
