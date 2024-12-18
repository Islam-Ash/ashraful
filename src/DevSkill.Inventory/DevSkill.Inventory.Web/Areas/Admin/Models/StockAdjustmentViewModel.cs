using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class StockAdjustmentViewModel
    {
        public DateTime Date { get; set; }

        // Foreign key for the specific StockItem being adjusted
        [Required]
        public Guid ItemId { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

        [Required]
        public int Quantity { get; set; }

        // Foreign key and navigation property for Reason
        [Required]
        public Guid ReasonId { get; set; }
        public IEnumerable<SelectListItem> Reasons { get; set; } = new List<SelectListItem>();
        [Required]
        public Guid WarehouseId { get; set; }
        public IEnumerable<SelectListItem> Warehouses { get; set; } = new List<SelectListItem>();
        public string AdjustedBy { get; set; }
        public string Note { get; set; }
    }
}
