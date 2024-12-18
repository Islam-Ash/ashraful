using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class StockItemCreateModel
	{
		public Guid WarehouseId { get; set; }

		[Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0.")]
		public int Quantity { get; set; }
	}
}
