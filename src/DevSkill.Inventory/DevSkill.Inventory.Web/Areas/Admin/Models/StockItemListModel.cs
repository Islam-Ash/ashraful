using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class StockItemListModel :DataTables
    {
		public StockItemSearchDto SearchStockItem { get; set; }
		public IList<SelectListItem> Categories { get; private set; }
		public IList<SelectListItem> Warehouses { get; private set; }
		public void SetCategoryValues(IList<Category> categories)
		{
			Categories = RazorUtility.ConvertCategories(categories);
		}
		public void SetWarehouseValues(IList<Warehouse> warehouses)
		{
			Warehouses = RazorUtility.ConvertWarehouses(warehouses);
		}
		//public Guid WarehouseId { get; set; }
		//      public string WarehouseName { get; set; }

		//      public int Quantity { get; set; }

	}
}
