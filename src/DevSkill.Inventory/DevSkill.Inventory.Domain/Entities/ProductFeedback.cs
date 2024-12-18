using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class ProductFeedback : IEntity<Guid>
    {
        public Guid Id { get; set ; }
    }
}
