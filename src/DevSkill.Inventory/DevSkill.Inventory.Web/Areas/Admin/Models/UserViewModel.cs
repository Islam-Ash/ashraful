using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
}
