using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class TaxCategoryDropdownItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public TaxCategoryDropdownItem() { }

        public TaxCategoryDropdownItem(string name, Guid id, decimal percentage)
        {
            Name = name;
            Id = id;
            Percentage = percentage;
        }
    }
}
