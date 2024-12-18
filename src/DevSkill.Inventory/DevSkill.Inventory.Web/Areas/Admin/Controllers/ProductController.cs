//using DevSkill.Inventory.Application.Services;
//using DevSkill.Inventory.Domain.Entities;
//using DevSkill.Inventory.Web.Areas.Admin.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Web;
//using DevSkill.Inventory.Infrastructure;
//using AutoMapper;

//namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductController : Controller
//    {
//        private readonly IProductManagementService _productManagementService;
//		private readonly ICategoryManagementService _categoryManagementService;
//		private readonly ILogger<ProductController> _logger;
//		private readonly IMapper _mapper;

//        public ProductController(ILogger<ProductController> logger,
//			IProductManagementService productManagementService,
//			ICategoryManagementService categoryManagementService,
//			IMapper mapper)
//        { 
//            _productManagementService = productManagementService;
//			_categoryManagementService = categoryManagementService;
//			_logger = logger;
//			_mapper = mapper;
//        }
       
//        public IActionResult Index()
//        {
//			var model = new ProductListModel();
//			model.SetCategoryValues(_categoryManagementService.GetCategories());
//			return View(model);
//        }

//		[HttpPost]
//        public JsonResult GetProductJsonData([FromBody] ProductListModel model)
//        {
//            var result = _productManagementService.GetProducts(model.PageIndex, model.PageSize, model.Search,
//				model.FormatSortExpression("ProductName", "Id"));

//            var productJsonData = new
//			{
//				recordsTotal = result.total,
//				recordsFiltered = result.totalDisplay,
//				data = (from record in result.data
//						select new string[]
//						{
//								HttpUtility.HtmlEncode(record.ProductName),
//								HttpUtility.HtmlEncode(record.Description),
//								HttpUtility.HtmlEncode(record.Price),
//								HttpUtility.HtmlEncode(record.Category?.Name),
//								record.Id.ToString()
//						}
//					).ToArray()
//			};

//			return Json(productJsonData);
//		}

//        public IActionResult Create()
//        {
//            var model = new ProductCreateModel();
//            model.SetCategoryValues(_categoryManagementService.GetCategories());
//            return View(model);
//        }

//        [HttpPost, ValidateAntiForgeryToken]
//        public IActionResult Create(ProductCreateModel model)
//        {
//			if (ModelState.IsValid)
//			{
//				var product = _mapper.Map<Product>(model);
//				product.Id = Guid.NewGuid();
//				product.Category = _categoryManagementService.GetCategory(model.CategoryId);
			
//				try
//				{
//					_productManagementService.CreateProduct(product);

//					TempData.Put("ResponseMessage", new ResponseModel
//					{
//						Message = "Product created successfuly",
//						Type = ResponseTypes.Success
//					});

//					return RedirectToAction("Index");
//				}
//				catch (Exception ex)
//				{
//					TempData.Put("ResponseMessage", new ResponseModel
//					{
//						Message = "Product creation failed",
//						Type = ResponseTypes.Danger
//					});

//					_logger.LogError(ex, "Product creation failed");
//				}
//			}
//            model.SetCategoryValues(_categoryManagementService.GetCategories());
//            return View(model);
//        }

//		public async Task<IActionResult> Update(Guid id)
//		{
//			Product product =await _productManagementService.GetProductAsync(id);
//			var model = _mapper.Map<ProductUpdateModel>(product);
//            model.SetCategoryValues(_categoryManagementService.GetCategories());

//            return View(model);
//		}

//		[HttpPost, ValidateAntiForgeryToken]
//		public async Task<IActionResult> Update(ProductUpdateModel model)
//		{
//			if (ModelState.IsValid)
//			{
//                var product = await _productManagementService.GetProductAsync(model.Id);
//                product = _mapper.Map(model, product);

//                product.Category = _categoryManagementService.GetCategory(model.CategoryId);
                


//                try
//                {
//					_productManagementService.UpdateProduct(product);

//					TempData.Put("ResponseMessage", new ResponseModel
//					{
//						Message = "Product updated successfuly",
//						Type = ResponseTypes.Success
//					});

//					return RedirectToAction("Index");
//				}
//				catch (Exception ex)
//				{
//					TempData.Put("ResponseMessage", new ResponseModel
//					{
//						Message = "Product update failed",
//						Type = ResponseTypes.Danger
//					});

//					_logger.LogError(ex, "Product update failed");
//				}
//			}

//            model.SetCategoryValues(_categoryManagementService.GetCategories());
//            return View(model);
//		}

//		[HttpPost, ValidateAntiForgeryToken]
//		public IActionResult Delete(Guid id)
//		{
//				try
//				{
//					_productManagementService.DeleteProduct(id);

//					TempData.Put("ResponseMessage", new ResponseModel
//					{
//						Message = "Product deleted successfuly",
//						Type = ResponseTypes.Success
//					});

//					return RedirectToAction("Index");
//				}
//				catch (Exception ex)
//				{
//					TempData.Put("ResponseMessage", new ResponseModel
//					{
//						Message = "Product delete failed",
//						Type = ResponseTypes.Danger
//					});

//					_logger.LogError(ex, "Product delete failed");
//				}

//			return View();
//		}
//	}
//}
