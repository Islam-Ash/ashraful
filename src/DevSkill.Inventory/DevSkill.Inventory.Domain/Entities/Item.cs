using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Item :IEntity<Guid>
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid CategoryId { get; set; }
		public Category Category { get; set; }
		public Guid MeasurementUnitId { get; set; }
		public Measurement MeasurementUnit { get; set; }
		public Guid BuyingTaxCategoryId { get; set; }
		public TaxCategory BuyingTaxCategory { get; set; }
		public Guid SellingTaxCategoryId { get; set; }
		public TaxCategory SellingTaxCategory { get; set; }
		public decimal BuyingPrice { get; set; }
		public decimal SellingPrice { get; set; }
		public string Barcode { get; set; }
		public int WarrantyDuration { get; set; }
		public string WarrantyUnit { get; set; }
		public string? Description { get; set; }
		public bool IsActive { get; set; }
		public bool IsSerialItem { get; set; }
		public string? PictureUrl { get; set; }

		// Navigation property for stock items
		public ICollection<StockItem> StockItems { get; set; }
	}
}
