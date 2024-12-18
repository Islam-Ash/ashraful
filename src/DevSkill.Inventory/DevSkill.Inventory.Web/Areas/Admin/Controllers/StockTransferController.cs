using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockTransferController : Controller
    {
        private readonly IStockTransferManagementService _stockTransferManagementService;
        private readonly IStockItemManagementService _stockItemManagementService;
        private readonly ILogger<StockTransferController> _logger;
        private readonly IItemManagementService _itemManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;

        public StockTransferController(ILogger<StockTransferController> logger,
            IStockTransferManagementService stockTransferManagementService,
            IStockItemManagementService stockItemManagementService,
            IItemManagementService itemManagementService,
            IWarehouseManagementService warehouseManagementService)
        {
            _logger = logger;
            _stockTransferManagementService = stockTransferManagementService;
            _stockItemManagementService = stockItemManagementService;
            _itemManagementService = itemManagementService;
            _warehouseManagementService = warehouseManagementService;
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetStockTransferJsonData([FromBody] StockTransferListModel model)
        {
            try
            {
                var result = await _stockTransferManagementService.GetStockTransfersAsync(
                    model.PageIndex,
                    model.PageSize,
                    model.Search,
                    model.FormatSortExpression("TransferDate", "SourceWarehouse", "DestinationWarehouse", "Item", "Quantity", "Note")
                );

                var stockTransferJsonData = new
                {
                    recordsTotal = result.total,
                    recordsFiltered = result.totalDisplay,
                    data = result.data.Select(record => new string[]
                    {
                       record.TransferDate.ToString("yyyy-MM-dd HH:mm:ss"),
                       HttpUtility.HtmlEncode (record.SourceWarehouse?.Name ?? "N/A"),
                       HttpUtility.HtmlEncode (record.DestinationWarehouse?.Name ?? "N/A"),
                       HttpUtility.HtmlEncode(record.Item?.Name ?? "No Item"),
                       record.Quantity.ToString(),
                       HttpUtility.HtmlEncode( record.Note ?? "N/A"),
                       record.Id.ToString()
                    }).ToArray()
                };
                return Json(stockTransferJsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching stock transfer data: {ex.Message}");

                return Json(new { success = false, message = "Error fetching stock transfer data. Please try again." });
            }
        }

        [HttpGet]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new StockTransferCreateModel();
            await PopulateStockTransferDropdownsAsync(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(StockTransferCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _stockTransferManagementService.TransferStockAsync(
                        model.ItemId, model.SourceWarehouseId, model.DestinationWarehouseId, model.TransferQuantity, model.Notes);

                    TempData["SuccessMessage"] = "Stock transferred successfully!";
                    return RedirectToAction("Index");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
                }
            }
            await PopulateStockTransferDropdownsAsync(model);
            return View(model);
        }


        private async Task PopulateStockTransferDropdownsAsync(StockTransferCreateModel model)
        {
            model.Warehouses = (await _warehouseManagementService.GetWarehousesAsync())
                .Select(w => new SelectListItem
                {
                    Value = w.Id.ToString(),
                    Text = w.Name
                }).ToList();

            model.Items = (await _itemManagementService.GetItemsAsync())
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                }).ToList();
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTransfer(Guid id)
        {
            await _stockTransferManagementService.DeleteTransferAsync(id);
            return Json(new { success = true, message = "Transfer deleted successfully." });
        }
    }
}
