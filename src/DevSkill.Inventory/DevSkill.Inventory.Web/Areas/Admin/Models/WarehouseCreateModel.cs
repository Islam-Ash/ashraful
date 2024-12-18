using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class WarehouseCreateModel
	{
	
		[Required, StringLength(100)]
		public string Name { get; set; }
	}
}
