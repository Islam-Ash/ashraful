using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using DevSkill.Inventory.Infrastructure;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
	[Area("Admin"),Authorize]
	public class WarehouseController : Controller
	{
		private readonly IWarehouseManagementService _warehouseManagementService;
		private readonly ILogger<WarehouseController> _logger;
		public WarehouseController(ILogger<WarehouseController> logger,
			IWarehouseManagementService warehouseManagementService)
		{
			_warehouseManagementService = warehouseManagementService;
			_logger = logger;
		}

		[Authorize(Roles = "SalesPerson,Admin")]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost, Authorize(Roles = "SalesPerson,Admin")]
		public async Task<JsonResult> GetWarehouseJsonData([FromBody] WarehouseListModel  model)
		{

			var result =  await _warehouseManagementService.GetWarehousesAsync(model.PageIndex,
				model.PageSize, model.Search, model.FormatSortExpression("Name","Id"));

			var warehouseJsonData = new
			{
				recordsTotal = result.total,
				recordsFiltered = result.totalDisplay,
				data = (from record in result.data
						select new string[]
						{
								HttpUtility.HtmlEncode(record.Name),
								record.Id.ToString()
						}
					).ToArray()
			};

			return Json(warehouseJsonData);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Create()
		{
			var model = new WarehouseCreateModel();
			return View(model);
		}


		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(WarehouseCreateModel model)
		{
			if (ModelState.IsValid)
			{
				var warehouse = new Warehouse
				{
					Id = Guid.NewGuid(),
					Name = model.Name
				};

				await _warehouseManagementService.CreateWarehouseAsync(warehouse);

				return Json(new { success = true, message = "Warehouse added successfully." });
			}
			return Json(new { success = false, message = "Validation failed." });
		}

		[Authorize(Roles = "Admin")]
		public async Task<ActionResult> Update(Guid id)
		{
			var model = new WarehouseUpdateModel();
			Warehouse warehouse = await _warehouseManagementService.GetWarehouseAsync(id);
			model.Name = warehouse.Name;
			model.Id = warehouse.Id;

			return View(model);
		}


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(WarehouseUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var warehouse = new Warehouse
                {
                    Id = model.Id,
                    Name = model.Name,
                   
                };

                try
                {
                   await _warehouseManagementService.UpdateWarehouseAsync(warehouse);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product updated successfuly",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse update failed",
                        Type = ResponseTypes.Danger
                    });

                    _logger.LogError(ex, "Warehouse update failed");
                }
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _warehouseManagementService.DeleteWarehouseAsync(id);
			return Json(new { success = true, message = "Warehouse deleted successfully." });
		}

        [HttpGet]
        public async Task<IActionResult> GetWarehouses()
        {
            try
            {
                var warehouses = await _warehouseManagementService.GetWarehousesAsync();
                var warehouseList = warehouses.Select(w => new { id = w.Id, text = w.Name }).ToList();

                return Json(new { success = true, data = warehouseList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching warehouses.");
                return Json(new { success = false, message = "An error occurred while fetching warehouses." });
            }
        }


    }
}
