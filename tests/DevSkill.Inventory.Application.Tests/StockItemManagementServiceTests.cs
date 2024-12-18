using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Application.Tests
{
	[ExcludeFromCodeCoverage]
	public class StockItemManagementServiceTests
	{
		private AutoMock _moq;
		private IStockItemManagementService _stockItemManagementService;
		private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
		private Mock<IStockItemRepository> _stockItemRepositoryMock;

		[SetUp]
		public void Setup()
		{
			_stockItemManagementService = _moq.Create<StockItemManagementService>();
			_inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
			_stockItemRepositoryMock = _moq.Mock<IStockItemRepository>();
		}

		[TearDown]
		public void Teardown()
		{
			_inventoryUnitOfWorkMock?.Reset();
			_stockItemRepositoryMock?.Reset();
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
		public async Task GetStockAdjustmentsAsync_ReturnsPagedStockAdjustments()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "Item1" }; // Example search term
			var order = "Quantity"; // Example order by field

			// Mock the data to return a paged response
			var stockAdjustments = new List<StockAdjustment>
			{
				new StockAdjustment
				{
					Id = Guid.NewGuid(),
					Date = DateTime.UtcNow,
					ItemId = Guid.NewGuid(),
					WarehouseId = Guid.NewGuid(),
					Quantity = 5,
					ReasonId = Guid.NewGuid(),
					AdjustedBy = "User1",
					Note = "Stock increased"
				}
			};

			var total = 2;
			var totalDisplay = 1;

			// Setup the mock to return filtered data
			_inventoryUnitOfWorkMock.Setup(x => x.StockItemRepository.GetPagedStockAdjustmentsAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((stockAdjustments, total, totalDisplay));

			// Act
			var result = await _stockItemManagementService.GetStockAdjustmentsAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(stockAdjustments.Count);
			result.total.ShouldBe(total);
			result.totalDisplay.ShouldBe(totalDisplay);

			// Assert that the data contains the expected category
			result.data.ShouldContain(c => c.AdjustedBy == "User1");
		}


		[Test]
		public async Task GetStockAdjustmentsAsync_ReturnsEmptyWhenNoStockAdjustmentMatchSearch()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "NonExistentStockItem" };
			var order = "Quantity";

			// Mock the data to return an empty list
			var services = new List<StockAdjustment>();
			var total = 0;
			var totalDisplay = 0;

			// Setup the mock to return empty data
			_inventoryUnitOfWorkMock.Setup(x => x.StockItemRepository.GetPagedStockAdjustmentsAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((services, total, totalDisplay));

			// Act
			var result = await _stockItemManagementService.GetStockAdjustmentsAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(0);
			result.total.ShouldBe(total);
			result.totalDisplay.ShouldBe(totalDisplay);
		}

		[Test]
		public async Task GetStockAdjustmentsAsync_ReturnsStockItemsWithOrdering()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "" };
			var order = "Quantity";

			var services = new List<StockAdjustment>
			{
				new StockAdjustment
				{
					Id = Guid.NewGuid(),
					Date = DateTime.UtcNow,
					ItemId = Guid.NewGuid(),
					WarehouseId = Guid.NewGuid(),
					Quantity = 5,
					ReasonId = Guid.NewGuid(),
					AdjustedBy = "User1",
					Note = "Stock increased"
				},
				new StockAdjustment
				{
					Id = Guid.NewGuid(),
					Date = DateTime.UtcNow,
					ItemId = Guid.NewGuid(),
					WarehouseId = Guid.NewGuid(),
					Quantity = -3,
					ReasonId = Guid.NewGuid(),
					AdjustedBy = "User2",
					Note = "Stock decreased"
				}
			};

			var total = 2;
			var totalDisplay = 2;

			// Setup the mock to return sorted data
			_inventoryUnitOfWorkMock.Setup(x => x.StockItemRepository.GetPagedStockAdjustmentsAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((services, total, totalDisplay));

			// Act
			var result = await _stockItemManagementService.GetStockAdjustmentsAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(services.Count);
			result.total.ShouldBe(total);
			result.totalDisplay.ShouldBe(totalDisplay);

			// Assert that the categories are ordered correctly by Name
			result.data.First().AdjustedBy.ShouldBe("User1");
			result.data.Last().AdjustedBy.ShouldBe("User2");
		}

	}
}