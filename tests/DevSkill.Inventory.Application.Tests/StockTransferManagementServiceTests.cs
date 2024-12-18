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
	public class StockTransferManagementServiceTests
	{
		private AutoMock _moq;
		private IStockTransferManagementService _stockTransferManagementService;
		private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
		private Mock<IStockTransferRepository> _stockTransferRepositoryMock;
		

		[SetUp]
		public void Setup()
		{
			_stockTransferManagementService = _moq.Create<StockTransferManagementService>();
			_inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
			_stockTransferRepositoryMock = _moq.Mock<IStockTransferRepository>();
		}

		[TearDown]
		public void Teardown()
		{
			_inventoryUnitOfWorkMock?.Reset();
			_stockTransferRepositoryMock?.Reset();
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
		public async Task DeleteTransferAsync_ValidStockTransferId_DeletesStockTransferHistory()
		{
			// Arrange
			var stockTransfer = Guid.NewGuid(); 

			_inventoryUnitOfWorkMock.Setup(x => x.StockTransferRepository).Returns(_stockTransferRepositoryMock.Object);
			_stockTransferRepositoryMock.Setup(x => x.RemoveAsync(stockTransfer)).Returns(Task.CompletedTask); // Simulate async remove
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask); // Simulate save after removal

			// Act
			await _stockTransferManagementService.DeleteTransferAsync(stockTransfer);

			// Assert
			_stockTransferRepositoryMock.Verify(x => x.RemoveAsync(stockTransfer));
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync());
		}
		
		[Test]
		public async Task TransferStockAsync_InsufficientStockInSourceWarehouse_ShouldThrowInvalidOperationException()
		{
			// Arrange
			var itemId = Guid.NewGuid();
			var sourceWarehouseId = Guid.NewGuid();
			var destinationWarehouseId = Guid.NewGuid();
			var transferQuantity = 10; 
			var notes = "Test transfer";

			_stockTransferRepositoryMock.Setup(repo => repo.GetTransferStockAsync(itemId, sourceWarehouseId, destinationWarehouseId, transferQuantity, notes))
										.ThrowsAsync(new InvalidOperationException("Insufficient stock in source warehouse for this transfer."));

			_inventoryUnitOfWorkMock.Setup(uow => uow.StockTransferRepository).Returns(_stockTransferRepositoryMock.Object);

			// Act & Assert
			var exception = Assert.ThrowsAsync<InvalidOperationException>(() =>
				_stockTransferManagementService.TransferStockAsync(itemId, sourceWarehouseId, destinationWarehouseId, transferQuantity, notes));

			exception.Message.ShouldBe("Insufficient stock in source warehouse for this transfer.");
		}

		[Test]
		public async Task TransferStockAsync_ValidTransfer_ShouldCallRepositoryMethod()
		{
			// Arrange
			var itemId = Guid.NewGuid();
			var sourceWarehouseId = Guid.NewGuid();
			var destinationWarehouseId = Guid.NewGuid();
			var transferQuantity = 5;
			var notes = "Test transfer";

			// Mock the GetTransferStockAsync method in the repository to not throw any exceptions
			_stockTransferRepositoryMock.Setup(repo => repo.GetTransferStockAsync(itemId, sourceWarehouseId, destinationWarehouseId, transferQuantity, notes))
										.Returns(Task.CompletedTask);

			// Mock the unit of work to return the mocked repository
			_inventoryUnitOfWorkMock.Setup(uow => uow.StockTransferRepository).Returns(_stockTransferRepositoryMock.Object);

			// Act
			await _stockTransferManagementService.TransferStockAsync(itemId, sourceWarehouseId, destinationWarehouseId, transferQuantity, notes);

			// Assert
			_stockTransferRepositoryMock.Verify(repo => repo.GetTransferStockAsync(itemId, sourceWarehouseId, destinationWarehouseId, transferQuantity, notes), Times.Once);
		}
		
		[Test]
		public async Task GetStockTransfersAsync_ReturnsPagedStockTransfers()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "Item1" }; // Example search term
			var order = "Quantity"; 

			// Mock the data to return a paged response
			var stockTransfers = new List<StockTransfer>
			{
				new StockTransfer
				{
					Id = Guid.NewGuid(),
					SourceWarehouseId = Guid.NewGuid(),
					DestinationWarehouseId = Guid.NewGuid(),
					ItemId = Guid.NewGuid(),
					Quantity = 5,
					TransferDate = DateTime.UtcNow,
					Note = "Stock transfer"
				}
			};

			var total = 2; 
			var totalDisplay = 1;

			// Setup the mock to return filtered data
			_inventoryUnitOfWorkMock.Setup(x => x.StockTransferRepository.GetPagedStockTransferAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((stockTransfers, total, totalDisplay));

			// Act
			var result = await _stockTransferManagementService.GetStockTransfersAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(stockTransfers.Count);
			result.total.ShouldBe(total); 
			result.totalDisplay.ShouldBe(totalDisplay); 

			// Assert that the data contains the expected category
			result.data.ShouldContain(c => c.Note == "Stock transfer");
		}


		[Test]
		public async Task GetCategoriesAsync_ReturnsEmptyWhenNoCategoriesMatchSearch()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "NonExistentCategory" }; // No category matches this search term
			var order = "Quantity";

			// Mock the data to return an empty list
			var stockTransfers = new List<StockTransfer>();
			var total = 0;
			var totalDisplay = 0;

			// Setup the mock to return empty data
			_inventoryUnitOfWorkMock.Setup(x => x.StockTransferRepository.GetPagedStockTransferAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((stockTransfers, total, totalDisplay));

			// Act
			var result = await _stockTransferManagementService.GetStockTransfersAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull(); 
			result.data.Count.ShouldBe(0); 
			result.total.ShouldBe(total); 
			result.totalDisplay.ShouldBe(totalDisplay); 
		}

		[Test]
		public async Task GetCategoriesAsync_ReturnsStockTransferWithOrdering()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "" }; 
			var order = "Quantity";

			var stockTransfers = new List<StockTransfer>
			{
				new StockTransfer
				{
					Id = Guid.NewGuid(),
					SourceWarehouseId = Guid.NewGuid(),
					DestinationWarehouseId = Guid.NewGuid(),
					ItemId = Guid.NewGuid(),
					Quantity = 5,
					TransferDate = DateTime.UtcNow,
					Note = "Stock transfer"
				},
				new StockTransfer
				{
					Id = Guid.NewGuid(),
					SourceWarehouseId = Guid.NewGuid(),
					DestinationWarehouseId = Guid.NewGuid(),
					ItemId = Guid.NewGuid(),
					Quantity = 10,
					TransferDate = DateTime.UtcNow,
					Note = "Stock transfer by van"
				}
			};

			var total = 2;
			var totalDisplay = 2; // All categories are displayed since no search term is used

			// Setup the mock to return sorted data
			_inventoryUnitOfWorkMock.Setup(x => x.StockTransferRepository.GetPagedStockTransferAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((stockTransfers, total, totalDisplay));

			// Act
			var result = await _stockTransferManagementService.GetStockTransfersAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(stockTransfers.Count); 
			result.total.ShouldBe(total);
			result.totalDisplay.ShouldBe(totalDisplay); 

			// Assert that the categories are ordered correctly by Name
			result.data.First().Quantity.ShouldBe(5);
			result.data.Last().Quantity.ShouldBe(10);
		}


	}
}