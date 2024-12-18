using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class StockTransferCreateModel
    {
		public Guid ItemId { get; set; }

		[Required(ErrorMessage = "Source warehouse is required.")]
		public Guid SourceWarehouseId { get; set; }

		[Required(ErrorMessage = "Destination warehouse is required.")]
		public Guid DestinationWarehouseId { get; set; }

		public int TransferQuantity { get; set; }

		[Required(ErrorMessage = "Transfer date is required.")]
		[DataType(DataType.Date)]
		public DateTime TransferDate { get; set; } = DateTime.Now;

		[StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
		public string Notes { get; set; }

		public List<SelectListItem> Warehouses { get; set; } = new List<SelectListItem>(); // Initialize list
		public List<SelectListItem> Items { get; set; } = new List<SelectListItem>();
	}
}
