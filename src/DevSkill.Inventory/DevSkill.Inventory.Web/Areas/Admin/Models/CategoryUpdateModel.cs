using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class CategoryUpdateModel
	{
		public Guid Id { get; set; }
		[Required, StringLength(100)]
		public string Name { get; set; }
		public string? Description { get; set; }
	}
}
