using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class StockItemDto
    {
        public Guid Id { get; set; }
		public string ItemName { get; set; }
		public string CategoryName { get; set; }
		public string WarehouseName { get; set; }
		public string Barcode { get; set; }
		public int Quantity { get; set; }
		public decimal BuyingPrice { get; set; }
		public DateTime AsOfDate { get; set; }
	}
}
