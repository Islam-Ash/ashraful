using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class ProductListModel: DataTables
	{
		public ItemSearchDto SearchItem { get; set; }
		public IList<SelectListItem> Categories {  get; private set; }

		public void SetCategoryValues(IList<Category> categories)
		{
			Categories = RazorUtility.ConvertCategories(categories);
		}
	}
}
