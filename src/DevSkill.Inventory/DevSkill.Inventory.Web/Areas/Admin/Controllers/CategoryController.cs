using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly ILogger<CategoryController> _logger;
       public CategoryController(ILogger<CategoryController> logger, 
           ICategoryManagementService categoryManagementService) 
        {
            _categoryManagementService = categoryManagementService;
            _logger = logger;
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<JsonResult> GetCategoryJsonData([FromBody]CategoryListModel model)
        {
            
            var result = await _categoryManagementService.GetCategoriesAsync(model.PageIndex, 
                model.PageSize, model.Search, model.FormatSortExpression("Name","Id"));

            var categoryJsonData = new
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
            
            return Json(categoryJsonData);
        }

        [Authorize(Roles = "SalesPerson,Admin")]
		public ActionResult Create()
		{
            var model = new CategoryCreateModel();
			return View(model);
		}

		
		[HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
		public async Task<IActionResult> Create(CategoryCreateModel model)
		{
			if (ModelState.IsValid)
			{
				var category = new Category
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
					Description = model.Description
				};

				await _categoryManagementService.CreateCategoryAsync(category);

				return Json(new { success = true, message = "Category added successfully." });
			}
			return Json(new { success = false, message = "Validation failed." });
        }

        [Authorize(Roles = "SalesPerson,Admin")]
        public async Task<ActionResult> Update(Guid id)
        {
            var model = new CategoryUpdateModel();
            Category category =await _categoryManagementService.GetCategoryAsync(id);
            model.Name = category.Name;
            model.Description = category.Description;
            model.Id = category.Id;

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<ActionResult> Update(CategoryUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description
                };

                try
                {
                    await _categoryManagementService.UpdateCategoryAsync(category);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category updated successfuly",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Category update failed",
                        Type = ResponseTypes.Danger
                    });

                    _logger.LogError(ex, "Category update failed");
                }
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "SalesPerson,Admin")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _categoryManagementService.DeleteCategoryAsync(id);
            return Json(new { success = true, message = "Category deleted successfully." });
        }

    }
}
