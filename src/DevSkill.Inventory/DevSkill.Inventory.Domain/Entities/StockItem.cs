using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockItem :IEntity<Guid>
    {
		public Guid Id { get; set; }
		public Guid ItemId { get; set; }
		public Item Item { get; set; }
		public Guid WarehouseId { get; set; }
		public Warehouse Warehouse { get; set; }
		public int Quantity { get; set; }
		public decimal CostPerUnit { get; set; }  // Set automatically from BuyingPrice
		public DateTime AsOfDate { get; set; }
	}
}
