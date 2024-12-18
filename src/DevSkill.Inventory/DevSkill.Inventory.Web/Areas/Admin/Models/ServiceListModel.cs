using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ServiceListModel : DataTables
    {
        public IList<SelectListItem>? TaxCategories { get; private set; }
        public void SetTaxCategoryValues(IList<TaxCategory> taxCategories)
        {
            TaxCategories = RazorUtility.ConvertTaxCategories(taxCategories);
        }
    }
}
