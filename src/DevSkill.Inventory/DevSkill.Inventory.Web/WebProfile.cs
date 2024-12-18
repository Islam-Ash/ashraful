using AutoMapper;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;

namespace DevSkill.Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ProductCreateModel, Product>().ReverseMap();
            CreateMap<ProductUpdateModel, Product>().ReverseMap();
          
            CreateMap<ServiceCreateModel, Service>()
                .ForMember(dest => dest.Price, opt => opt.Ignore()) // Assuming Price is computed
                .ForMember(dest => dest.TaxCategory, opt => opt.Ignore()) // Will set manually
                .ForMember(dest => dest.BuyingPriceTaxed, opt => opt.Ignore()) // If computed
                .ForMember(dest => dest.SellingPriceTaxed, opt => opt.Ignore()) // If computed
                .ReverseMap()
                .ForMember(dest => dest.TaxCategories, opt => opt.Ignore());

            CreateMap<ServiceUpdateModel, Service>()
                  .ForMember(dest => dest.Price, opt => opt.Ignore()) // Assuming Price is computed or not directly set
                  .ForMember(dest => dest.BuyingPriceTaxed, opt => opt.Ignore()) // If computed
                  .ForMember(dest => dest.SellingPriceTaxed, opt => opt.Ignore()) // If computed
                  .ForMember(dest => dest.TaxCategory, opt => opt.Ignore()) // Will set manually
                  .ReverseMap()
                  .ForMember(dest => dest.TaxCategoriesForDropdown, opt => opt.Ignore()); // Updated this to match the property in ServiceUpdateModel
                                                                                          // Ignoring TaxCategories for reverse mapping

            CreateMap<ItemCreateModel, Item>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())) // Generate new ID
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore()) // Ignore PictureUrl if not set here
                .ForMember(dest => dest.StockItems, opt => opt.MapFrom(src =>
                  src.StockItems.Select(s => new StockItem
                  {
                     Id = Guid.NewGuid(), // Generate new ID for StockItem
                     WarehouseId = s.WarehouseId,
                     Quantity = s.Quantity,
                     AsOfDate = src.AsOfDate
                  })));
            CreateMap<Item, ItemCreateModel>()
                .ForMember(dest => dest.Picture, opt => opt.Ignore()) // Ignore Picture property as it’s a file
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.MeasurementUnits, opt => opt.Ignore())
                .ForMember(dest => dest.BuyingTaxCategories, opt => opt.Ignore())
                .ForMember(dest => dest.SellingTaxCategories, opt => opt.Ignore())
                .ForMember(dest => dest.Warehouses, opt => opt.Ignore());

            CreateMap<ItemUpdateModel, Item>()
                .ForMember(dest => dest.StockItems, opt => opt.MapFrom(src =>
                    src.StockItems.Select(s => new StockItem
                    {
                        WarehouseId = s.WarehouseId,
                        Quantity = s.Quantity,
                        AsOfDate = src.AsOfDate,
						 CostPerUnit = src.BuyingPrice
					})))
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());

            CreateMap<Item, ItemUpdateModel>()
                .ForMember(dest => dest.Picture, opt => opt.Ignore()) // Ignore properties not present in the entity
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.MeasurementUnits, opt => opt.Ignore())
                .ForMember(dest => dest.BuyingTaxCategories, opt => opt.Ignore())
                .ForMember(dest => dest.SellingTaxCategories, opt => opt.Ignore())
                .ForMember(dest => dest.Warehouses, opt => opt.Ignore());


            CreateMap<Item, ItemDto>()
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name)) // Map CategoryName
                 .ForMember(dest => dest.SellingTaxPercentage,
                            opt => opt.MapFrom(src => src.SellingTaxCategory.Percentage)) // Map SellingTaxPercentage from SellingTaxCategory
                 .ForMember(dest => dest.Quantity,
                            opt => opt.MapFrom(src => src.StockItems.Sum(s => s.Quantity))) // Pre-aggregated quantity from StockItems
                 .ForMember(dest => dest.PictureUrl,
                            opt => opt.MapFrom(src => src.PictureUrl ?? "/images/default.jpg")) // Provide default picture URL if null
                 .ForMember(dest => dest.IsActive,
                            opt => opt.MapFrom(src => src.IsActive)); // Map IsActive

			CreateMap<StockAdjustmentViewModel, StockAdjustment>()
				 .ForMember(dest => dest.Id, opt => opt.Ignore())
				 .ForMember(dest => dest.AdjustedBy, opt => opt.Ignore())
	             .ReverseMap();
		    CreateMap<StockItem, StockItemDto>()
	             .ForMember(dest => dest.BuyingPrice, opt => opt.MapFrom(src => src.Item.BuyingPrice));


		}
	}
}
