using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ReasonController : Controller
    {
        private readonly IReasonManagementService _reasonManagementService;
        private readonly ILogger<ReasonController> _logger;

        public ReasonController(ILogger<ReasonController> logger,
            IReasonManagementService reasonManagementService)
        {
            _reasonManagementService = reasonManagementService;
            _logger = logger;
        }

        public IActionResult Create()
        {
            var model = new ReasonCreateModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReasonCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var reason = new Reason
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                await _reasonManagementService.CreateReasonAsync(reason);

                return Json(new { success = true, id = reason.Id, name = reason.Name });
            }

            return Json(new { success = false });
        }


        [HttpGet]
        public async Task<IActionResult> GetReasons()
        {
            try
            {
                var reasons = await _reasonManagementService.GetReasonsAsync();
                var reasonList = reasons.Select(r => new { id = r.Id, text = r.Name }).ToList();

                return Json(new { success = true, data = reasonList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reasons.");
                return Json(new { success = false, message = "An error occurred while fetching reasons." });
            }
        }
    }
}
