using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class MeasurementUpdateModel
    {
        public Guid Id { get; set; }
        [Required, StringLength(100)]
        public string MeasurementName { get; set; }
        [Required, StringLength(100)]
        public string Symbol { get; set; }
    }
}
