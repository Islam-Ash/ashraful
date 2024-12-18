using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class ItemUpdateModel
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Item Name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Category is required.")]
		public Guid CategoryId { get; set; }
		public List<SelectListItem>? Categories { get; set; }

		[Required(ErrorMessage = "Measurement Unit is required.")]
		public Guid MeasurementUnitId { get; set; }
		public List<SelectListItem>? MeasurementUnits { get; set; }

		[Required(ErrorMessage = "Buying Tax is required.")]
		public Guid BuyingTaxCategoryId { get; set; }
		public List<SelectListItem>? BuyingTaxCategories { get; set; }

		[Required(ErrorMessage = "Selling Tax is required.")]
		public Guid SellingTaxCategoryId { get; set; }
		public List<SelectListItem>? SellingTaxCategories { get; set; }

		[Required(ErrorMessage = "Buying Price is required.")]
		[Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
		public decimal BuyingPrice { get; set; }

		[Required(ErrorMessage = "Selling Price is required.")]
		[Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
		public decimal SellingPrice { get; set; }
		public decimal? CostPerUnit { get; set; }

		[Required(ErrorMessage = "Barcode is required.")]
		public string Barcode { get; set; }

		public int WarrantyDuration { get; set; }
		public string WarrantyUnit { get; set; }

		public string? Description { get; set; }

		public bool IsActive { get; set; }
		public bool IsSerialItem { get; set; }

		// This could be included if you want to allow updating the picture
		[Display(Name = "Picture (Leave empty to keep existing)")]
		public IFormFile? Picture { get; set; } // Optional for updating the image

		public List<SelectListItem>? Warehouses { get; set; } = new List<SelectListItem>(); // Default to empty list if null

		public List<StockItemModel>? StockItems { get; set; } = new List<StockItemModel>(); // Default to empty list if null

		public DateTime AsOfDate { get; set; } = DateTime.Now; // Default to current date
	}

}
