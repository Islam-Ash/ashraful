using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
	public class TaxCategory : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Percentage {  get; set; }

		// Navigation properties for items
		public ICollection<Item> BuyingItems { get; set; }
		public ICollection<Item> SellingItems { get; set; }
	}
}
