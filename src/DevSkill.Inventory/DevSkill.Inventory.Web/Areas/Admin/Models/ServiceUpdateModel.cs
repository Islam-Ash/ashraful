using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ServiceUpdateModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Service Name is required.")]
        [StringLength(100, ErrorMessage = "Service Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Buying Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Buying Price must be a positive value.")]
        public decimal BuyingPrice { get; set; }

        [Required(ErrorMessage = "Selling Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Selling Price must be a positive value.")]
        public decimal SellingPrice { get; set; }

        public decimal BuyingPriceTaxed { get; set; }
        public decimal SellingPriceTaxed { get; set; }

        [Required(ErrorMessage = "Tax Category is required.")]
        public Guid TaxCategoryId { get; set; }

        public bool IsPurchased { get; set; }
        public bool IsSold { get; set; }

        public bool IsBuyingPriceTaxInclusive { get; set; }
        public bool IsSellingPriceTaxInclusive { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public IList<SelectListItem>? TaxCategories { get; private set; }
        public IList<TaxCategoryDropdownItem>? TaxCategoriesForDropdown { get; private set; }

        public void SetTaxCategoryValues(IList<TaxCategory> taxCategories)
        {
            TaxCategories = RazorUtility.ConvertTaxCategories(taxCategories);
            TaxCategoriesForDropdown = RazorUtility.ConvertTaxCategoriesForDropdown(taxCategories);
        }
    }
}
