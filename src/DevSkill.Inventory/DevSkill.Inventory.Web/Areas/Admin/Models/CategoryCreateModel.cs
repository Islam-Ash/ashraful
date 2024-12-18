using DevSkill.Inventory.Domain;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class CategoryCreateModel
	{
		[Required, StringLength(100)]
		public string Name { get; set; }
		public string? Description { get; set; }
	}
}
