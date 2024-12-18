using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductCreateModel
    {
        [Required, StringLength(100)]
        public string ProductName { get; set; }
        [Required, StringLength(100)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Category"), Required]
        public Guid CategoryId { get; set; }
        public IList<SelectListItem>? Categories { get; private set; }

        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.ConvertCategories(categories);
        }
    }
}
