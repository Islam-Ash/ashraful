using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MeasurementUnitController : Controller
    {
        private readonly IMeasurementManagementService _measurementManagementService;
        private readonly ILogger<MeasurementUnitController> _logger;
        public MeasurementUnitController(ILogger<MeasurementUnitController> logger,
            IMeasurementManagementService measurementManagementService)
        {
            _measurementManagementService = measurementManagementService;
            _logger = logger;
        }
        [Authorize(Roles = "SalesPerson,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetMeasurementUnitJsonData([FromBody] MeasurementListModel model)
        {

            var result = await _measurementManagementService.GetMeasurementUnitsAsync(model.PageIndex,
                model.PageSize, model.Search, model.FormatSortExpression("MeasurementName", "Symbol", "Id"));

            var MeasurementUnitJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.MeasurementName),
                                HttpUtility.HtmlEncode(record.Symbol),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };

            return Json(MeasurementUnitJsonData);
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public ActionResult Create()
        {
            var model = new MeasurementCreateModel();
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Create(MeasurementCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var measurementUnit = new Measurement
                {
                    Id = Guid.NewGuid(),
                    MeasurementName = model.MeasurementName,
                    Symbol = model.Symbol
                };

                await _measurementManagementService.CreateMeasurementUnitAsync(measurementUnit);

                return Json(new { success = true, message = "Measurement Unit added successfully." });
            }
            return Json(new { success = false, message = "Validation failed." });
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public async Task<ActionResult> Update(Guid id)
        {
            var model = new MeasurementUpdateModel();
            Measurement measurementUnit = await _measurementManagementService.GetMeasurementUnitAsync(id);
            model.MeasurementName = measurementUnit.MeasurementName;
            model.Symbol = measurementUnit.Symbol;
            model.Id = measurementUnit.Id;

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Update(MeasurementUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var measurementUnit = new Measurement
                {
                    Id = model.Id,
                    MeasurementName = model.MeasurementName,
                    Symbol = model.Symbol
                };

                try
                {
                    await _measurementManagementService.UpdateMeasurmentUnitAsync(measurementUnit);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Measurment Unit updated successfuly",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Measurment Unit update failed",
                        Type = ResponseTypes.Danger
                    });

                    _logger.LogError(ex, "Measurment Unit update failed");
                }
            }

            return View();
        }

        [HttpPost,ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
               await _measurementManagementService.DeleteMeasurementUnitAsync(id);
                return Json(new { success = true, message = "Measurement Unit deleted successfully." });
            
        }


    }
}
