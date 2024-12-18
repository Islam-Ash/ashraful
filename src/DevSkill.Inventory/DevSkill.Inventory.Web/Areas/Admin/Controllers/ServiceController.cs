using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Data;
namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class ServiceController : Controller
    {
        private readonly IServicesManagementService _servicesManagementService;
        private readonly ITaxCategoryManagementService _taxCategoryManagementService;
        private readonly ILogger<ServiceController> _logger;
        private readonly IMapper _mapper;
        public ServiceController(ILogger<ServiceController> logger,
            IServicesManagementService servicesManagementService,
            ITaxCategoryManagementService taxCategoryManagementService, IMapper mapper)
        {
            _servicesManagementService = servicesManagementService;
            _taxCategoryManagementService = taxCategoryManagementService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetServiceJsonData([FromBody] ServiceListModel model)
        {
            var result = await _servicesManagementService.GetServicesAsync(model.PageIndex, model.PageSize, model.Search,
                model.FormatSortExpression("Name", "TaxCategory", "SellingPriceTaxed", "Id"));

            var serviceJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(record.Name),
                            HttpUtility.HtmlEncode(record.TaxCategory != null
                                ? $"{record.TaxCategory.Percentage:F2}%"
                                : "N/A"),

                            HttpUtility.HtmlEncode(record.SellingPriceTaxed != null
                                ? $"{record.SellingPriceTaxed:C2}"
                                : "N/A"),
                            record.Id.ToString()
                        }
                ).ToArray()
            };

            return Json(serviceJsonData);
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public async Task<ActionResult> Create()
        {
            var model = new ServiceCreateModel();
            model.SetTaxCategoryValues(await _taxCategoryManagementService.GetTaxCategoriesAsync());
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Create(ServiceCreateModel model)
        {

            if (ModelState.IsValid)
            {
                var service = _mapper.Map<Service>(model);
                service.Id = Guid.NewGuid();
                var taxCategory = await _taxCategoryManagementService.GetTaxCategoryAsync(model.TaxCategoryId);
                if (taxCategory == null)
                {
                    ModelState.AddModelError("TaxCategoryId", "Selected Tax Category does not exist.");
                    // Re-populate TaxCategories before returning the view
                    model.SetTaxCategoryValues(await _taxCategoryManagementService.GetTaxCategoriesAsync());
                    return View(model);
                }

                service.TaxCategory = taxCategory;

                // Calculate taxed prices using tax rate from TaxCategory
                service.BuyingPriceTaxed = _servicesManagementService.CalculateTaxedPrice(model.IsBuyingPriceTaxInclusive, model.BuyingPrice, taxCategory.Percentage);
                service.SellingPriceTaxed = _servicesManagementService.CalculateTaxedPrice(model.IsSellingPriceTaxInclusive, model.SellingPrice, taxCategory.Percentage);
                try
                {
                    await _servicesManagementService.CreateServiceAsync(service);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Service created successfuly",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Service creation failed",
                        Type = ResponseTypes.Danger
                    });

                    _logger.LogError(ex, "Service creation failed");
                }
            }
            model.SetTaxCategoryValues(await _taxCategoryManagementService.GetTaxCategoriesAsync());
            return View(model);
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Update(Guid id)
        {
            Service service = await _servicesManagementService.GetServiceAsync(id);
            var model = _mapper.Map<ServiceUpdateModel>(service);
            model.SetTaxCategoryValues(await _taxCategoryManagementService.GetTaxCategoriesAsync());

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<IActionResult> Update(ServiceUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _servicesManagementService.GetServiceAsync(model.Id);
                service = _mapper.Map(model, service);

                var taxCategory = await _taxCategoryManagementService.GetTaxCategoryAsync(model.TaxCategoryId);
                if (taxCategory == null)
                {
                    ModelState.AddModelError("TaxCategoryId", "Selected Tax Category does not exist.");
                    model.SetTaxCategoryValues(await _taxCategoryManagementService.GetTaxCategoriesAsync());
                    return View(model);
                }

                service.TaxCategory = taxCategory;
                service.BuyingPriceTaxed = _servicesManagementService.CalculateTaxedPrice(model.IsBuyingPriceTaxInclusive, model.BuyingPrice, taxCategory.Percentage);
                service.SellingPriceTaxed = _servicesManagementService.CalculateTaxedPrice(model.IsSellingPriceTaxInclusive, model.SellingPrice, taxCategory.Percentage);

                try
                {
                    await _servicesManagementService.UpdateServiceAsync(service);
                    TempData["SuccessMessage"] = "Item added successfully!";
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Service updated successfully",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Service update failed",
                        Type = ResponseTypes.Danger
                    });

                    _logger.LogError(ex, "Service update failed");
                }
            }

            model.SetTaxCategoryValues(await _taxCategoryManagementService.GetTaxCategoriesAsync());
            return View(model);
        }





        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _servicesManagementService.DeleteServiceAsync(id);
            return Json(new { success = true, message = "Service deleted successfully." });
        }


    }
}
