using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockAdjustment : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Foreign key for the item being adjusted
        public Guid ItemId { get; set; }
        public Item Item { get; set; }  // Navigation property for Item

        // Foreign key for the warehouse where adjustment is taking place
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } // Navigation property for Warehouse

        public int Quantity { get; set; }

        // Foreign key and navigation property for Reason
        public Guid ReasonId { get; set; }
        public Reason Reason { get; set; }

        public string AdjustedBy { get; set; }
        public string Note { get; set; }
    }

}
