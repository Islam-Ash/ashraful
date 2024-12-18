using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
	public interface IReasonManagementService
    {
        Task<IList<Reason>> GetReasonsAsync();
		Task CreateReasonAsync(Reason reason);
       
    }
}
