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
	public class WarehouseManagementServiceTests
	{
		private AutoMock _moq;
		private IWarehouseManagementService _warehouseManagementService;
		private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
		private Mock<IWarehouseRepository> _warehouseRepositoryMock;

		[SetUp]
		public void Setup()
		{
			_warehouseManagementService = _moq.Create<WarehouseManagementService>();
			_inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
			_warehouseRepositoryMock = _moq.Mock<IWarehouseRepository>();
		}

		[TearDown]
		public void Teardown()
		{
			_inventoryUnitOfWorkMock?.Reset();
			_warehouseRepositoryMock?.Reset();
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
		public async Task CreateWarehouseAsync_TitleNotDuplicate_CreatesWarehouse()
		{
			// Arrange
			var warehouse = new Warehouse
			{
				Name = "Warehouse1",
				StockItems = new List<StockItem>()
			};


			_inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository).Returns(_warehouseRepositoryMock.Object);
			_warehouseRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(warehouse.Name, null)).ReturnsAsync(false);
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();
			_warehouseRepositoryMock.Setup(x => x.AddAsync(warehouse)).Verifiable();

			//Act
			await _warehouseManagementService.CreateWarehouseAsync(warehouse);

			// Assert
			_warehouseRepositoryMock.VerifyAll();
			_inventoryUnitOfWorkMock.VerifyAll();
		}

		[Test]
		public async Task UpdateWarehouseAsync_WithDuplicateTitle_DoesNotUpdate()
		{
			var warehouse = new Warehouse
			{
				Id = Guid.NewGuid(),
				Name = "Warehouse2",
				StockItems = new List<StockItem>()
			};

			// Mock the repository methods
			_inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository).Returns(_warehouseRepositoryMock.Object);
			_warehouseRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(warehouse.Name, warehouse.Id)).ReturnsAsync(true);

			// Act
			await _warehouseManagementService.UpdateWarehouseAsync(warehouse);
			// Assert
			_warehouseRepositoryMock.Verify(x => x.EditAsync(It.IsAny<Warehouse>()));
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync());
		}


		[Test]
		public async Task DeleteWarehouseAsync_ValidWarehouseId_DeletesWarehouse()
		{
			// Arrange
			var warehouseId = Guid.NewGuid(); 

			// Setup mocks for repository and unit of work
			_inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository).Returns(_warehouseRepositoryMock.Object);
			_warehouseRepositoryMock.Setup(x => x.RemoveAsync(warehouseId)).Returns(Task.CompletedTask); // Simulate async remove
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask); // Simulate save after removal

			// Act
			await _warehouseManagementService.DeleteWarehouseAsync(warehouseId);

			// Assert
			_warehouseRepositoryMock.Verify(x => x.RemoveAsync(warehouseId));
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync());
		}

		[Test]
		public async Task GetWarehouseAsync_ValidCategoryId_ReturnsCategory()
		{
			// Arrange
			var warehouseId = Guid.NewGuid(); 
			var expectedWarehouse = new Warehouse
			{
				Id = Guid.NewGuid(),
				Name = "Warehouse2",
				StockItems = new List<StockItem>()
			};

			_inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository.GetByIdAsync(warehouseId))
									.ReturnsAsync(expectedWarehouse);

			// Act
			var result = await _warehouseManagementService.GetWarehouseAsync(warehouseId);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe(expectedWarehouse.Id);
			result.Name.ShouldBe(expectedWarehouse.Name);
		}

		[Test]
		public async Task GetWarehousesAsync_ReturnsListOfWarehouses()
		{
			// Arrange
			var warehouses = new List<Warehouse>
			{
				new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse1", StockItems = new List<StockItem>() },
				new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse2", StockItems = new List<StockItem>() }
			};

			// Setup the mock to return the list of categories
			_inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository.GetAllAsync())
									.ReturnsAsync(warehouses);

			// Act
			var result = await _warehouseManagementService.GetWarehousesAsync();

			// Assert
			result.ShouldNotBeNull(); 
			result.Count.ShouldBe(warehouses.Count); 
			result.ShouldContain(c => c.Name == "Warehouse1"); 
			result.ShouldContain(c => c.Name == "Warehouse2"); 
		}

		[Test]
		public async Task GetWarehousesAsync_ReturnsEmptyList_WhenNoWarehouses()
		{
			// Arrange
			var warehouses = new List<Warehouse>();

			_inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository.GetAllAsync())
									.ReturnsAsync(warehouses);

			// Act
			var result = await _warehouseManagementService.GetWarehousesAsync();

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(0);
		}

		[Test]
		public async Task GetWarehousesAsync_ReturnsPagedWarehouses()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "Warehouse1" }; // Example search term
			var order = "Name"; 

			// Mock the data to return a paged response
			var warehouses = new List<Warehouse>
			{
				new Warehouse { Id = Guid.NewGuid(), Name = "Warehouse1", StockItems = new List<StockItem>() }
			};

			var total = 2; 
			var totalDisplay = 1; 

			// Setup the mock to return filtered data
			_inventoryUnitOfWorkMock.Setup(x => x.WarehouseRepository.GetPagedWarehousesAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((warehouses, total, totalDisplay));

			// Act
			var result = await _warehouseManagementService.GetWarehousesAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(warehouses.Count); 
			result.total.ShouldBe(total); 
			result.totalDisplay.ShouldBe(totalDisplay); 

			// Assert that the data contains the expected category
			result.data.ShouldContain(c => c.Name == "Warehouse1");
		}
	}
}