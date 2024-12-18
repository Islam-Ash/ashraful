using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockTransfer : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid SourceWarehouseId { get; set; }
        public Guid DestinationWarehouseId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;
        public string Note { get; set; }

        // Navigation properties
        public Warehouse SourceWarehouse { get; set; }
        public Warehouse DestinationWarehouse { get; set; }
        public Item Item { get; set; }
    }
}
