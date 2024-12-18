using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Application.Tests
{
	[ExcludeFromCodeCoverage]
	public class ServicesManagementServiceTests
	{
		private AutoMock _moq;
		private IServicesManagementService _servicesManagementService;
		private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
		private Mock<IServiceRepository> _serviceRepositoryMock;

		[SetUp]
		public void Setup()
		{
			_servicesManagementService = _moq.Create<ServicesManagementService>();
			_inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
			_serviceRepositoryMock = _moq.Mock<IServiceRepository>();
		}

		[TearDown]
		public void Teardown()
		{
			_inventoryUnitOfWorkMock?.Reset();
			_serviceRepositoryMock?.Reset();
		}

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_moq = AutoMock.GetLoose();
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			_moq?.Dispose();
		}

		[Test]
		public async Task CreateServiceAsync_TitleNotDuplicate_CreatesService()
		{
			// Arrange
			var service = new Service
			{
				Id = Guid.NewGuid(),
				Name = "Repearing",
				Price = 100,
				TaxCategoryId = Guid.NewGuid(),
				TaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "Tax Free" },
				IsPurchased = true,
				IsSold = false,
				IsBuyingPriceTaxInclusive = true,
				IsSellingPriceTaxInclusive = true,
				Description = "Description for Repearing"
			};


			_inventoryUnitOfWorkMock.Setup(x => x.ServiceRepository).Returns(_serviceRepositoryMock.Object);
			_serviceRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(service.Name, null)).ReturnsAsync(false);
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();
			_serviceRepositoryMock.Setup(x => x.AddAsync(service)).Verifiable();

			//Act
			await _servicesManagementService.CreateServiceAsync(service);

			// Assert
			_serviceRepositoryMock.VerifyAll();
			_inventoryUnitOfWorkMock.VerifyAll();
		}

		[Test]
		public async Task UpdateServiceAsync_WithDuplicateTitle_DoesNotUpdate()
		{
			// Arrange
			var service = new Service
			{
				Id = Guid.NewGuid(),
				Name = "Repearing",
				Price = 100,
				TaxCategoryId = Guid.NewGuid(),
				TaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "Tax Free" },
				IsPurchased = true,
				IsSold = false,
				IsBuyingPriceTaxInclusive = true,
				IsSellingPriceTaxInclusive = true,
				Description = "Description for Repearing"
			};

			// Mock the repository methods
			_inventoryUnitOfWorkMock.Setup(x => x.ServiceRepository).Returns(_serviceRepositoryMock.Object);
			_serviceRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(service.Name, service.Id)).ReturnsAsync(true);

			// Act
			await _servicesManagementService.UpdateServiceAsync(service);
			// Assert
			_serviceRepositoryMock.Verify(x => x.EditAsync(It.IsAny<Service>()), Times.Never);
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Never);
		}


		[Test]
		public async Task DeleteServiceAsync_ValidServiceId_DeletesService()
		{
			// Arrange
			var serviceId = Guid.NewGuid(); 

			// Setup mocks for repository and unit of work
			_inventoryUnitOfWorkMock.Setup(x => x.ServiceRepository).Returns(_serviceRepositoryMock.Object);
			_serviceRepositoryMock.Setup(x => x.RemoveAsync(serviceId)).Returns(Task.CompletedTask); // Simulate async remove
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask); // Simulate save after removal

			// Act
			await _servicesManagementService.DeleteServiceAsync(serviceId);

			// Assert
			_serviceRepositoryMock.Verify(x => x.RemoveAsync(serviceId));
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync());
		}

		[Test]
		public async Task GetServiceAsync_ValidServiceId_ReturnsService()
		{
			// Arrange
			var serviceId = Guid.NewGuid();  // Create a new Guid for the category ID
			var expectedService = new Service
			{
				Id = Guid.NewGuid(),
				Name = "Repearing",
				Price = 100,
				TaxCategoryId = Guid.NewGuid(),
				TaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "Tax Free" },
				IsPurchased = true,
				IsSold = false,
				IsBuyingPriceTaxInclusive = true,
				IsSellingPriceTaxInclusive = true,
				Description = "Description for Repearing"
			};


			_inventoryUnitOfWorkMock.Setup(x => x.ServiceRepository.GetByIdAsync(serviceId))
									.ReturnsAsync(expectedService);

			// Act
			var result = await _servicesManagementService.GetServiceAsync(serviceId);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe(expectedService.Id);
			result.Name.ShouldBe(expectedService.Name);
			result.Description.ShouldBe(expectedService.Description);
		}

		[Test]
		public async Task GetServicesAsync_ReturnsPagedServices()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "Repearing" }; // Example search term
			var order = "Name"; // Example order by field

			// Mock the data to return a paged response
			var services = new List<Service>
			{
				new Service
				{
					Id = Guid.NewGuid(),
					Name = "Repearing",
					Price = 100,
					TaxCategoryId = Guid.NewGuid(),
					TaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "Tax Free" },
					IsPurchased = true,
					IsSold = false,
					IsBuyingPriceTaxInclusive = true,
					IsSellingPriceTaxInclusive = true,
					Description = "Description for Repearing"
				}
			};
		
			var total = 2; 
			var totalDisplay = 1; 

			// Setup the mock to return filtered data
			_inventoryUnitOfWorkMock.Setup(x => x.ServiceRepository.GetPagedServicesAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((services, total, totalDisplay));

			// Act
			var result = await _servicesManagementService.GetServicesAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(services.Count); 
			result.total.ShouldBe(total); 
			result.totalDisplay.ShouldBe(totalDisplay);

			// Assert that the data contains the expected category
			result.data.ShouldContain(c => c.Name == "Repearing");
		}


		[Test]
		public async Task GetServicesAsync_ReturnsEmptyWhenNoServicesMatchSearch()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "NonExistentCategory" }; 
			var order = "Name";

			// Mock the data to return an empty list
			var services = new List<Service>();
			var total = 0;
			var totalDisplay = 0;

			// Setup the mock to return empty data
			_inventoryUnitOfWorkMock.Setup(x => x.ServiceRepository.GetPagedServicesAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((services, total, totalDisplay));

			// Act
			var result = await _servicesManagementService.GetServicesAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull(); 
			result.data.Count.ShouldBe(0); 
			result.total.ShouldBe(total); 
			result.totalDisplay.ShouldBe(totalDisplay); 
		}

		[Test]
		public async Task GetServicesAsync_ReturnsServicesWithOrdering()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "" }; 
			var order = "Name";

			var services = new List<Service>
			{
				new Service
				{
					Id = Guid.NewGuid(),
					Name = "Repearing",
					Price = 100,
					TaxCategoryId = Guid.NewGuid(),
					TaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "Tax Free" },
					IsPurchased = true,
					IsSold = false,
					IsBuyingPriceTaxInclusive = true,
					IsSellingPriceTaxInclusive = true,
					Description = "Description for Repearing"
				},

				new Service
				{
					Id = Guid.NewGuid(),
					Name = "Electric Service",
					Price = 100,
					TaxCategoryId = Guid.NewGuid(),
					TaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "Tax Free" },
					IsPurchased = true,
					IsSold = false,
					IsBuyingPriceTaxInclusive = true,
					IsSellingPriceTaxInclusive = true,
					Description = "Description for Repearing"
				}
			};

			var total = 2;
			var totalDisplay = 2; 

			// Setup the mock to return sorted data
			_inventoryUnitOfWorkMock.Setup(x => x.ServiceRepository.GetPagedServicesAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((services, total, totalDisplay));

			// Act
			var result = await _servicesManagementService.GetServicesAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(services.Count); 
			result.total.ShouldBe(total); 
			result.totalDisplay.ShouldBe(totalDisplay);

			// Assert that the categories are ordered correctly by Name
			result.data.First().Name.ShouldBe("Repearing");
			result.data.Last().Name.ShouldBe("Electric Service");
		}

		[Test]
		public void CalculateTaxedPrice_WithInclusiveTax_ReturnsCorrectPrice()
		{
			// Arrange
			var price = 100m;
			var taxPercentage = 10m; 
			var isInclusive = true;
			var expectedPrice = 90.91m; 

			// Act
			var result = _servicesManagementService.CalculateTaxedPrice(isInclusive, price, taxPercentage);

			// Assert
			Assert.AreEqual(expectedPrice, result);
		}

		[Test]
		public void CalculateTaxedPrice_WithNonInclusiveTax_ReturnsCorrectPrice()
		{
			// Arrange
			var price = 100m;
			var taxPercentage = 10m; // 10% tax
			var isInclusive = false;
			var expectedPrice = 110m; // Price including tax (100 * (1 + 0.10))

			// Act
			var result = _servicesManagementService.CalculateTaxedPrice(isInclusive, price, taxPercentage);

			// Assert
			Assert.AreEqual(expectedPrice, result);
		}

		[Test]
		public void CalculateTaxedPrice_WithZeroTax_ReturnsOriginalPrice()
		{
			// Arrange
			var price = 100m;
			var taxPercentage = 0m;
			var isInclusive = false;
			var expectedPrice = 100m;

			// Act
			var result = _servicesManagementService.CalculateTaxedPrice(isInclusive, price, taxPercentage);

			// Assert
			Assert.AreEqual(expectedPrice, result);
		}
	}
}