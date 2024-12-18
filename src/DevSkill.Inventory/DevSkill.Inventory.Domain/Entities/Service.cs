using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Service : IEntity<Guid>
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; } // This might be a computed property
		public Guid TaxCategoryId { get; set; }
		public TaxCategory TaxCategory { get; set; }
		public bool IsPurchased { get; set; }
		public bool IsSold { get; set; }
		public bool IsBuyingPriceTaxInclusive { get; set; }
		public bool IsSellingPriceTaxInclusive { get; set; }
		public string? Description { get; set; }

		// Optional: Computed properties or other business logic
		public decimal BuyingPriceTaxed { get; set; }
		public decimal SellingPriceTaxed { get; set; }
	}
}
