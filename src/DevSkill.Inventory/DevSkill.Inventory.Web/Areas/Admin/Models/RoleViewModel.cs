﻿using DevSkill.Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class RoleViewModel
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string NormalizedName { get; set; }
	}
}