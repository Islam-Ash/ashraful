using Autofac.Extras.Moq;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Moq;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace DevSkill.Inventory.Application.Tests
{
	[ExcludeFromCodeCoverage]
	public class ItemManagementServiceTests
	{
		private AutoMock _moq;
		private IItemManagementService _itemManagementService;
		private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
		private Mock<IItemRepository> _itemRepositoryMock;

		[SetUp]
		public void Setup()
		{
			_itemManagementService = _moq.Create<ItemManagementService>();
			_inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
			_itemRepositoryMock = _moq.Mock<IItemRepository>();
		}

		[TearDown]
		public void Teardown() 
		{
			_inventoryUnitOfWorkMock?.Reset();
			_itemRepositoryMock?.Reset();
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
		public async Task CreateItemAsync_TitleNotDuplicate_CreatesItem()
		{
			// Arrange
			var item = new Item
			{
				Name = "Coffee",
				Category = new Category { Id = Guid.NewGuid(), Name = "Beverages" },
				MeasurementUnit = new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram" },
				BuyingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 15%" },
				SellingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 7%" },
				BuyingPrice = 100,
				SellingPrice = 150,
				Barcode = "1234567890123",
				WarrantyDuration = 12,
				WarrantyUnit = "Months",
				Description = "Premium coffee beans.",
				IsActive = true,
				IsSerialItem = false,
				PictureUrl = "/images/items/coffee.png",
				StockItems = new List<StockItem>()
			};


			_inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
			_itemRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(item.Name, null)).ReturnsAsync(false);
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();
			_itemRepositoryMock.Setup(x => x.AddAsync(item)).Verifiable();

			//Act
			await _itemManagementService.CreateItemAsync(item);

			// Assert
			_itemRepositoryMock.VerifyAll();
			_inventoryUnitOfWorkMock.VerifyAll();
		}

		[Test]
		public async Task UpdateItemAsync_DuplicateTitle_ThrowsException()
		{
			// Arrange
			var item = new Item
			{
				Id = Guid.NewGuid(),
				Name = "Coffee",
				Category = new Category { Id = Guid.NewGuid(), Name = "Beverages" },
				MeasurementUnit = new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram" },
				BuyingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 15%" },
				SellingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 7%" },
				BuyingPrice = 100,
				SellingPrice = 150,
				Barcode = "1234567890123",
				WarrantyDuration = 12,
				WarrantyUnit = "Months",
				Description = "Premium coffee beans.",
				IsActive = true,
				IsSerialItem = false,
				PictureUrl = "/images/items/coffee.png",
				StockItems = new List<StockItem>()
			};
			var error = "Title Should be Unique";

			// Setup mock repository
			_inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
			_itemRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(item.Name, item.Id)).ReturnsAsync(true);

			// Act & Assert
			var exception = await Should.ThrowAsync<InvalidOperationException>(() => _itemManagementService.UpdateItemAsync(item));

			// Assert
			exception.Message.ShouldBe(error);
		}

		[Test]
		public async Task DeleteItemAsync_ItemExists_DeletesItem()
		{
			// Arrange
			var itemId = Guid.NewGuid();
			var itemToDelete = new Item
			{
				Id = itemId,
				Name = "Coffee",
				Category = new Category { Id = Guid.NewGuid(), Name = "Beverages" },
				MeasurementUnit = new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram" },
				BuyingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 15%" },
				SellingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 7%" },
				BuyingPrice = 100,
				SellingPrice = 150,
				Barcode = "1234567890123",
				WarrantyDuration = 12,
				WarrantyUnit = "Months",
				Description = "Premium coffee beans.",
				IsActive = true,
				IsSerialItem = false,
				PictureUrl = "/images/items/coffee.png",
				StockItems = new List<StockItem>()
			};

			// Setting up mocks
			_inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
			_itemRepositoryMock.Setup(x => x.GetItemAsync(itemId)).ReturnsAsync(itemToDelete);
			_itemRepositoryMock.Setup(x => x.RemoveAsync(itemId)).Verifiable(); // Remove by ID
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable(); // Use SaveAsync

			// Act
			await _itemManagementService.DeleteItemAsync(itemId);

			// Assert
			_itemRepositoryMock.Verify(x => x.RemoveAsync(itemId), Times.Once); // Ensure RemoveAsync is called once
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once); // Ensure SaveAsync is called once
		}

		[Test]
		public async Task GetItemAsync_ItemExists_ReturnsItem()
		{
			// Arrange
			var itemId = Guid.NewGuid();
			var expectedItem = new Item
			{
				Id = itemId,
				Name = "Coffee",
				Category = new Category { Id = Guid.NewGuid(), Name = "Beverages" },
				MeasurementUnit = new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram" },
				BuyingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 15%" },
				SellingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 7%" },
				BuyingPrice = 100,
				SellingPrice = 150,
				Barcode = "1234567890123",
				WarrantyDuration = 12,
				WarrantyUnit = "Months",
				Description = "Premium coffee beans.",
				IsActive = true,
				IsSerialItem = false,
				PictureUrl = "/images/items/coffee.png",
				StockItems = new List<StockItem>()
			};

			_inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
			_itemRepositoryMock.Setup(x => x.GetItemAsync(itemId)).ReturnsAsync(expectedItem);

			// Act
			var result = await _itemManagementService.GetItemAsync(itemId);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe(itemId);
			result.Name.ShouldBe(expectedItem.Name);
			result.Category.Name.ShouldBe(expectedItem.Category.Name);
			_itemRepositoryMock.Verify(x => x.GetItemAsync(itemId), Times.Once);
		}

		[Test]
		public async Task GetItemAsync_ItemNotFound_ReturnsNull()
		{
			// Arrange
			var itemId = Guid.NewGuid();

			_inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
			_itemRepositoryMock.Setup(x => x.GetItemAsync(itemId)).ReturnsAsync((Item)null);

			// Act
			var result = await _itemManagementService.GetItemAsync(itemId);

			// Assert
			result.ShouldBeNull();
			_itemRepositoryMock.Verify(x => x.GetItemAsync(itemId), Times.Once);
		}

		[Test]
		public async Task GetItemsAsync_ReturnsItems()
		{
			// Arrange
			var expectedItems = new List<Item>
			{
				new Item
				{
					Id = Guid.NewGuid(),
					Name = "Coffee",
					Category = new Category { Id = Guid.NewGuid(), Name = "Beverages" },
					MeasurementUnit = new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram" },
					BuyingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 15%" },
					SellingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 7%" },
					BuyingPrice = 100,
					SellingPrice = 150,
					Barcode = "1234567890123",
					WarrantyDuration = 12,
					WarrantyUnit = "Months",
					Description = "Premium coffee beans.",
					IsActive = true,
					IsSerialItem = false,
					PictureUrl = "/images/items/coffee.png",
					StockItems = new List<StockItem>()
				},
				new Item
				{
					Id = Guid.NewGuid(),
					Name = "Tea",
					Category = new Category { Id = Guid.NewGuid(), Name = "Beverages" },
					MeasurementUnit = new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram" },
					BuyingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 10%" },
					SellingTaxCategory = new TaxCategory { Id = Guid.NewGuid(), Name = "VAT 7%" },
					BuyingPrice = 50,
					SellingPrice = 80,
					Barcode = "9876543210987",
					WarrantyDuration = 12,
					WarrantyUnit = "Months",
					Description = "Premium tea leaves.",
					IsActive = true,
					IsSerialItem = false,
					PictureUrl = "/images/items/tea.png",
					StockItems = new List<StockItem>()
				}
			};

			_inventoryUnitOfWorkMock.Setup(x => x.ItemRepository).Returns(_itemRepositoryMock.Object);
			_itemRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expectedItems);

			// Act
			var result = await _itemManagementService.GetItemsAsync();

			// Assert
			result.ShouldBe(expectedItems); 
			_itemRepositoryMock.Verify(x => x.GetAllAsync()); 
		}

	}
}