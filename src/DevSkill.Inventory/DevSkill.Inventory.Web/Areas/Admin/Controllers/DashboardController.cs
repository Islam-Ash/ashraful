﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin"), Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}