using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class MeasurementCreateModel
    {
        [Required, StringLength(100)]
        public string MeasurementName { get; set; }
        public string Symbol { get; set; }
    }
}
