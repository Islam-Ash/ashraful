using Autofac;
using DevSkill.Inventory.Application;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Repositories;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using DevSkill.Inventory.Web.Data;
using DevSkill.Inventory.Web.Models;

namespace DevSkill.Inventory.Web
{
    //primary Constructor used
    public class WebModule(string connectionString, string migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           
            builder.RegisterType<InventoryDbContext>().AsSelf()
               .WithParameter("connectionString", connectionString)
               .WithParameter("migrationAssembly", migrationAssembly)
               .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();

			builder.RegisterType<ProductManagementService>()
				.As<IProductManagementService>()
				.InstancePerLifetimeScope();

			builder.RegisterType<InventoryUnitOfWork>()
               .As<IInventoryUnitOfWork>()
               .InstancePerLifetimeScope();

			builder.RegisterType<CategoryRepository>()
			   .As<ICategoryRepository>()
			   .InstancePerLifetimeScope();

			builder.RegisterType<CategoryManagementService>()
			   .As<ICategoryManagementService>()
			   .InstancePerLifetimeScope();

			builder.RegisterType<MeasurementRepository>()
	           .As<IMeasurementRepository>()
	           .InstancePerLifetimeScope();

			builder.RegisterType<MeasurementManagementService>()
			   .As<IMeasurementManagementService>()
			   .InstancePerLifetimeScope();

			builder.RegisterType<TaxCategoryRepository>()
			   .As<ITaxCategoryRepository>()
			   .InstancePerLifetimeScope();

			builder.RegisterType<TaxCategoryManagementService>()
			   .As<ITaxCategoryManagementService>()
			   .InstancePerLifetimeScope();

            builder.RegisterType<ServiceRepository>()
               .As<IServiceRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<ServicesManagementService>()
               .As<IServicesManagementService>()
               .InstancePerLifetimeScope();

			builder.RegisterType<WarehouseRepository>()
			  .As<IWarehouseRepository>()
			  .InstancePerLifetimeScope();

			builder.RegisterType<WarehouseManagementService>()
			   .As<IWarehouseManagementService>()
			   .InstancePerLifetimeScope();

            builder.RegisterType<ItemRepository>()
              .As<IItemRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<ItemManagementService>()
               .As<IItemManagementService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<StockItemRepository>()
            .As<IStockItemRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterType<StockItemManagementService>()
               .As<IStockItemManagementService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<StockTransferRepository>()
           .As<IStockTransferRepository>()
           .InstancePerLifetimeScope();

            builder.RegisterType<StockTransferManagementService>()
               .As<IStockTransferManagementService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<ReasonRepository>()
               .As<IReasonRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<ReasonManagementService>()
               .As<IReasonManagementService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<EmailUtility>()
			  .As<IEmailUtility>()
			  .InstancePerLifetimeScope();


		}
    }
}
