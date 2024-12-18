using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using DevSkill.Inventory.Worker;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ItemController : Controller
    {
        private readonly IItemManagementService _itemManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly ITaxCategoryManagementService _taxCategoryManagementService;
        private readonly IStockItemManagementService _stockItemManagementService;
        private readonly IMeasurementManagementService _measurementManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly ILogger<ItemController> _logger;
        private readonly IMapper _mapper;

		private readonly ISQSService _sqsService;

		public ItemController(ILogger<ItemController> logger, IMapper mapper,
           IItemManagementService itemManagementService,
            ICategoryManagementService categoryManagementService,
            ITaxCategoryManagementService taxCategoryManagementService,
            IStockItemManagementService stockItemManagementService,
            IWarehouseManagementService warehouseManagementService,
            IMeasurementManagementService measurementManagementService,
			 ISQSService sqsService)
        {
            _itemManagementService = itemManagementService;
            _categoryManagementService = categoryManagementService;
            _taxCategoryManagementService = taxCategoryManagementService;
            _measurementManagementService = measurementManagementService;
            _stockItemManagementService = stockItemManagementService;
            _warehouseManagementService = warehouseManagementService;
            _logger = logger;
            _mapper = mapper;
			_sqsService = sqsService;

        }
        [Authorize(Roles = "SalesPerson,Admin")]
        public IActionResult Index()
        {
            var model = new ItemListModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            return View(model);
        }

        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetItemJsonData([FromBody] ItemListModel model)
        {
            try
            {
                var result = await _itemManagementService.GetItemsSPAsync(
                    model.PageIndex,
                    model.PageSize,
                    model.SearchItem,
                    model.FormatSortExpression("Name", "Barcode", "CategoryName", "SellingPrice", "SellingTaxPercentage", "Quantity", "IsActive", "Id")
                );
                var itemJsonData = new
                {
                    recordsTotal = result.total,
                    recordsFiltered = result.totalDisplay,
                    data = result.data.Select(record => new string[]
                    {
                            record.PictureUrl ?? "/images/default.jpg",
                            HttpUtility.HtmlEncode(record.Name),
                            HttpUtility.HtmlEncode(record.Barcode),
                            HttpUtility.HtmlEncode(record.CategoryName ?? "No Category"),
                            record.SellingPrice.ToString("F2"),
                            record.SellingTaxPercentage.HasValue ? record.SellingTaxPercentage.Value.ToString("F2") : "No Tax",
                            record.Quantity.ToString(), // Quantity is pre-aggregated in stored procedure
							record.IsActive ? "Active" : "Inactive",
                            record.Id.ToString()
                    }).ToArray()
                };

                return Json(itemJsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving item data");
                return Json(new { error = "An error occurred while retrieving item data." });
            }
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new ItemCreateModel();
            await PopulateDropdownsAsync(model);
            return View(model);
        }


		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
		public async Task<IActionResult> Create(ItemCreateModel model)
		{
			if (ModelState.IsValid)
			{
				string pictureUrl = null;

				if (model.Picture != null && model.Picture.Length > 0)
				{
					var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

					if (!Directory.Exists(uploadDirectory))
					{
						Directory.CreateDirectory(uploadDirectory);
					}

					var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Picture.FileName);
					var filePath = Path.Combine(uploadDirectory, fileName);

					// Upload the original image
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await model.Picture.CopyToAsync(stream);
					}

					// Prepare the data to send to SQS
					var resizeData = new
					{
						FilePath = filePath,
						Width = 300,
						Height = 300
					};

					// Serialize the data to JSON string
					string jsonMessage = JsonConvert.SerializeObject(resizeData);

					// Send the message to the SQS service
					string queueUrl = "https://sqs.us-east-1.amazonaws.com/847888492411/ashraful-b10-queue";
					await _sqsService.SendMessageAsync(queueUrl, jsonMessage);

					// Save the original image URL for the item
					pictureUrl = "/images/" + fileName;

					var newItem = _mapper.Map<Item>(model);
					newItem.PictureUrl = pictureUrl;

					await _itemManagementService.CreateItemAsync(newItem);
					TempData["SuccessMessage"] = "Item added successfully!";

					return RedirectToAction("Index");
				}

				await PopulateDropdownsAsync(model);
				return View(model);
			}

			await PopulateDropdownsAsync(model);
			return View(model);
		}

		private async Task PopulateDropdownsAsync(ItemCreateModel model)
        {
            model.Categories = (await _categoryManagementService.GetCategoriesAsync()).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            model.MeasurementUnits = (await _measurementManagementService.GetMeasurementUnitsAsync()).Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.MeasurementName
            }).ToList();

            model.BuyingTaxCategories = (await _taxCategoryManagementService.GetTaxCategoriesAsync()).Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.Name} ({t.Percentage}%)"
            }).ToList();

            model.SellingTaxCategories = (await _taxCategoryManagementService.GetTaxCategoriesAsync()).Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.Name} ({t.Percentage}%)"
            }).ToList();

            model.Warehouses = (await _warehouseManagementService.GetWarehousesAsync()).Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.Name
            }).ToList();

            model.StockItems = new List<StockItemModel>();

        }

        [HttpGet, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Update(Guid id)
        {

            var existingItem = await _itemManagementService.GetItemAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ItemUpdateModel>(existingItem);
            await PopulateDropdownsAsync(model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Update(ItemUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                string pictureUrl = null;

                if (model.Picture != null && model.Picture.Length > 0)
                {
                    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Picture.FileName);
                    var filePath = Path.Combine(uploadDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Picture.CopyToAsync(stream);
                    }

                    pictureUrl = "/images/" + fileName;

                }

                var existingItem = await _itemManagementService.GetItemAsync(model.Id);
                if (existingItem == null)
                {
                    return NotFound();
                }

                _mapper.Map(model, existingItem);
                existingItem.PictureUrl = pictureUrl ?? existingItem.PictureUrl;

                await _itemManagementService.UpdateItemAsync(existingItem);
                TempData["SuccessMessage"] = "Item updated successfully!";

                return RedirectToAction("Index");
            }

            await PopulateDropdownsAsync(model);
            return View(model);
        }

        private async Task PopulateDropdownsAsync(ItemUpdateModel model)
        {
            model.Categories = (await _categoryManagementService.GetCategoriesAsync()).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            model.MeasurementUnits = (await _measurementManagementService.GetMeasurementUnitsAsync()).Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.MeasurementName
            }).ToList();

            model.BuyingTaxCategories = (await _taxCategoryManagementService.GetTaxCategoriesAsync()).Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.Name} ({t.Percentage}%)"
            }).ToList();

            model.SellingTaxCategories = (await _taxCategoryManagementService.GetTaxCategoriesAsync()).Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.Name} ({t.Percentage}%)"
            }).ToList();

            model.Warehouses = (await _warehouseManagementService.GetWarehousesAsync()).Select(w => new SelectListItem
            {
                Value = w.Id.ToString(),
                Text = w.Name
            }).ToList();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _itemManagementService.DeleteItemAsync(id);
            return Json(new { success = true, message = "Item deleted successfully." });
        }


    }


}

