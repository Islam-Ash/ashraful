using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
	[Area("Admin"), Authorize]
	public class TaxCategoryController : Controller
	{
		private readonly ITaxCategoryManagementService _taxCategoryManagenmentService;
		private readonly ILogger<TaxCategoryController> _logger;
		public TaxCategoryController(ILogger<TaxCategoryController> logger,
			ITaxCategoryManagementService taxCategoryManagenmentService)
		{
			_taxCategoryManagenmentService = taxCategoryManagenmentService;
			_logger = logger;
		}

        [Authorize(Roles = "SalesPerson,Admin")]
        public IActionResult Index()
		{
			return View();
		}

        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetTaxCategoryJsonData([FromBody] TaxCategoryListModel model)
        {

            var result = await _taxCategoryManagenmentService.GetTaxCategoriesAsync(model.PageIndex,
                model.PageSize, model.Search, model.FormatSortExpression("Name", "Percentage"));

            var TaxCategoryJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Percentage),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };

            return Json(TaxCategoryJsonData);
        }
    }
}
