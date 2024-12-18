using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class RazorUtility
    {
        public static IList<SelectListItem> ConvertCategories(IList<Category> categories)
        {
            var items = (from c in categories
                          select new SelectListItem(c.Name, c.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select a Category", string.Empty));

            return items;
        }
		public static IList<SelectListItem> ConvertWarehouses(IList<Warehouse> warehouses)
		{
			var items = (from c in warehouses
						 select new SelectListItem(c.Name, c.Id.ToString()))
						  .ToList();

			items.Insert(0, new SelectListItem("Select a Warehouse", string.Empty));

			return items;
		}


		public static IList<SelectListItem> ConvertTaxCategories(IList<TaxCategory> taxCategories)
        {
            var items = (from c in taxCategories
                         select new SelectListItem(c.Name, c.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select a Tax Category", string.Empty));

            return items;
        }

        public static IList<SelectListItem> ConvertMeasurementUnits(IList<Measurement> measurementUnits)
        {
            var items = (from c in measurementUnits
                         select new SelectListItem(c.MeasurementName, c.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select a MeasurementUnit", string.Empty));

            return items;
        }
        public static IList<TaxCategoryDropdownItem> ConvertTaxCategoriesForDropdown(IList<TaxCategory> taxCategories)
        {
            var items = (from c in taxCategories
                         select new TaxCategoryDropdownItem
                         {
                             Name = c.Name,
                             Id = c.Id,  // Assuming c.Id is a Guid
                             Percentage = c.Percentage // Assuming c.Percentage is the tax rate
                         })
                        .ToList();

            // Insert a default item at the beginning
            items.Insert(0, new TaxCategoryDropdownItem
            {
                Name = "Select a Tax Category",
                Id = Guid.Empty, // Using an empty Guid to represent the default selection
                Percentage = 0 // Default percentage for the placeholder option
            });

            return items;
        }
    }
}
