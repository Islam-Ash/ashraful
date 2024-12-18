using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockItemController : Controller
    {
        private readonly IStockItemManagementService _stockItemManagementService;
        private readonly ILogger<StockItemController> _logger;
        private readonly IItemManagementService _itemManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IReasonManagementService _reasonManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IMapper _mapper;
        public StockItemController(ILogger<StockItemController> logger,
            IStockItemManagementService stockItemManagementService,
            IItemManagementService itemManagementService,
            IWarehouseManagementService warehouseManagementService,
            IReasonManagementService reasonManagementService,
            ICategoryManagementService categoryManagementService, IMapper mapper)
        {
            _logger = logger;
            _stockItemManagementService = stockItemManagementService;
            _itemManagementService = itemManagementService;
            _warehouseManagementService = warehouseManagementService;
            _reasonManagementService = reasonManagementService;
            _categoryManagementService = categoryManagementService;
            _mapper = mapper;
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Index()
        {
            var model = new StockItemListModel();
            model.SetCategoryValues(await _categoryManagementService.GetCategoriesAsync());
            model.SetWarehouseValues(await _warehouseManagementService.GetWarehousesAsync());

            return View(model);
        }


        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetStockItemJsonData([FromBody] StockItemListModel model)
        {
            try
            {
                _logger.LogInformation("Fetching stock items with parameters: PageIndex={PageIndex}, PageSize={PageSize}, Search={Search}",
                                       model.PageIndex, model.PageSize, model.SearchStockItem);

                var result = await _stockItemManagementService.GetStockItemsSPAsync(
                    model.PageIndex,
                    model.PageSize,
                    model.SearchStockItem,
                    model.FormatSortExpression("ItemName", "Barcode", "WarehouseName", "Quantity", "CostPerUnit", "Id")
                );

                if (result.data == null || !result.data.Any())
                {
                    _logger.LogWarning("No data found for the stock items.");
                }

                var stockItemJsonData = new
                {
                    recordsTotal = result.total,
                    recordsFiltered = result.totalDisplay,
                    data = result.data.Select(record => new string[]
                    {
                    HttpUtility.HtmlEncode(record.ItemName ?? "No Item"),
                    HttpUtility.HtmlEncode(record.Barcode ?? "N/A"),
                    HttpUtility.HtmlEncode(record.WarehouseName ?? "N/A"),
                    record.Quantity.ToString("N0"),
                    record.BuyingPrice.ToString("F2"),
                    record.Id.ToString()
                    }).ToArray()
                };

                return Json(stockItemJsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stock items.");
                return Json(new { error = "An error occurred while fetching stock items." });
            }
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public IActionResult StockAdjustmentIndex()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetStockAdjustmentJsonData([FromBody] StockItemAdjustmentModel model)
        {
            try
            {
                var result = await _stockItemManagementService.GetStockAdjustmentsAsync(model.PageIndex, model.PageSize, model.Search,
                    model.FormatSortExpression("Date", "Item", "Warehouse", "Quantity", "Reason", "AdjustedBy", "Note", "Id"));

                var stockAdjustmentJsonData = new
                {
                    recordsTotal = result.total,
                    recordsFiltered = result.totalDisplay,
                    data = (from record in result.data
                            select new string[]
                            {
                                record.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                                HttpUtility.HtmlEncode(record.Item?.Name ?? "No Item"),
                                HttpUtility.HtmlEncode(record.Warehouse?.Name ?? "N/A"),
                                record.Quantity.ToString(),
                                HttpUtility.HtmlEncode(record.Reason.Name?? "N/A"),
                                HttpUtility.HtmlEncode( record.AdjustedBy ?? "N/A"),
                                HttpUtility.HtmlEncode( record.Note ?? "N/A"),
                                record.Id.ToString()
                            }
                    ).ToArray()
                };

                return Json(stockAdjustmentJsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stock adjustment items");
                return Json(new { error = "An error occurred while fetching adjustment items." });
            }
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new StockAdjustmentViewModel
            {
                Date = DateTime.Now,
                AdjustedBy = User.Identity?.Name
            };

            await PopulateDropdownsAsync(model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StockAdjustmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model);
                return View(model);
            }

            try
            {
                var stockAdjustment = _mapper.Map<StockAdjustment>(model);

                stockAdjustment.Id = Guid.NewGuid();
                stockAdjustment.AdjustedBy = User.Identity?.Name ?? "System";
                stockAdjustment.Date = DateTime.UtcNow;

                await _stockItemManagementService.CreateforUpdateAsync(stockAdjustment);

                TempData["SuccessMessage"] = "Stock adjustment has been saved successfully.";
                return RedirectToAction("StockAdjustmentIndex");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the stock adjustment.");

                TempData["ErrorMessage"] = "An unexpected error occurred while saving the stock adjustment. Please try again.";
                return RedirectToAction("StockAdjustmentIndex");
            }
        }

        private async Task PopulateDropdownsAsync(StockAdjustmentViewModel model)
        {
            model.Items = (await _itemManagementService.GetItemsAsync())
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                }).ToList();

            model.Warehouses = (await _warehouseManagementService.GetWarehousesAsync())
                .Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = w.Name
                }).ToList();

            model.Reasons = (await _reasonManagementService.GetReasonsAsync())
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                }).ToList();
        }

        public async Task<JsonResult> GetItemsByWarehouse(Guid warehouseId)
        {
            try
            {
                var stockItems = await _stockItemManagementService.GetStockItemsAsyncByWarehouse(warehouseId);
                var items = stockItems.Select(si => new
                {
                    id = si.ItemId,
                    name = si.Item.Name,
                    quantity = si.Quantity
                });

                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching items for warehouse {warehouseId}: {ex.Message}");
                return Json(new { success = false, message = "Error fetching items. Please try again." });
            }
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            _stockItemManagementService.DeleteAdjustment(id);
            return Json(new { success = true, message = "Stock Adjustment deleted successfully." });
        }


    }
}
