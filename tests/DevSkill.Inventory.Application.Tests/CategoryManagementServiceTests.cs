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
	public class CategoryManagementServiceTests
	{
		private AutoMock _moq;
		private ICategoryManagementService _categoryManagementService;
		private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
		private Mock<ICategoryRepository> _categoryRepositoryMock;

		[SetUp]
		public void Setup()
		{
			_categoryManagementService = _moq.Create<CategoryManagementService>();
			_inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
			_categoryRepositoryMock = _moq.Mock<ICategoryRepository>();
		}

		[TearDown]
		public void Teardown()
		{
			_inventoryUnitOfWorkMock?.Reset();
			_categoryRepositoryMock?.Reset();
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
		public async Task CreateCategory_TitleNotDuplicate_CreatesCategory()
		{
			// Arrange
			var category = new Category
			{
				Name = "Beverages",
				Description = "Premium coffee beans.",
				Items = new List<Item>()
			};


			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository).Returns(_categoryRepositoryMock.Object);
			_categoryRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(category.Name, null)).ReturnsAsync(false);
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();
			_categoryRepositoryMock.Setup(x => x.AddAsync(category)).Verifiable();

			//Act
			await _categoryManagementService.CreateCategoryAsync(category);

			// Assert
			_categoryRepositoryMock.VerifyAll();
			_inventoryUnitOfWorkMock.VerifyAll();
		}

		[Test]
		public async Task UpdateCategoryAsync_WithDuplicateTitle_DoesNotUpdate()
		{
			// Arrange
			var category = new Category
			{
				Id = Guid.NewGuid(),  // Ensure the category has an ID for the update scenario
				Name = "Beverages",
				Description = "Premium coffee beans.",
				Items = new List<Item>()
			};

			// Mock the repository methods
			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository).Returns(_categoryRepositoryMock.Object);
			_categoryRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(category.Name, category.Id)).ReturnsAsync(true);

			// Act
			await _categoryManagementService.UpdateCategoryAsync(category); 
			// Assert
			_categoryRepositoryMock.Verify(x => x.EditAsync(It.IsAny<Category>()), Times.Never);  
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync(), Times.Never);  
		}


		[Test]
		public async Task DeleteCategoryAsync_ValidCategoryId_DeletesCategory()
		{
			// Arrange
			var categoryId = Guid.NewGuid(); // Create a new Guid to represent a category ID.

			// Setup mocks for repository and unit of work
			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository).Returns(_categoryRepositoryMock.Object);
			_categoryRepositoryMock.Setup(x => x.RemoveAsync(categoryId)).Returns(Task.CompletedTask); // Simulate async remove
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask); // Simulate save after removal

			// Act
			await _categoryManagementService.DeleteCategoryAsync(categoryId);

			// Assert
			_categoryRepositoryMock.Verify(x => x.RemoveAsync(categoryId));
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync());
		}

		[Test]
		public async Task GetCategoryAsync_ValidCategoryId_ReturnsCategory()
		{
			// Arrange
			var categoryId = Guid.NewGuid(); // Create a new Guid for the category ID
			var expectedCategory = new Category
			{
				Id = categoryId,
				Name = "Beverages",
				Description = "Premium coffee beans.",
				Items = new List<Item>()
			};

			
			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository.GetByIdAsync(categoryId))
									.ReturnsAsync(expectedCategory);

			// Act
			var result = await _categoryManagementService.GetCategoryAsync(categoryId);

			// Assert
			result.ShouldNotBeNull(); 
			result.Id.ShouldBe(expectedCategory.Id); 
			result.Name.ShouldBe(expectedCategory.Name); 
			result.Description.ShouldBe(expectedCategory.Description); 
		}

		[Test]
		public async Task GetCategoriesAsync_ReturnsListOfCategories()
		{
			// Arrange
			var categories = new List<Category>
			{
				new Category { Id = Guid.NewGuid(), Name = "Beverages", Description = "Premium coffee beans." },
				new Category { Id = Guid.NewGuid(), Name = "Snacks", Description = "Chips and crackers." }
			};

			// Setup the mock to return the list of categories
			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync())
									.ReturnsAsync(categories);

			// Act
			var result = await _categoryManagementService.GetCategoriesAsync();

			// Assert
			result.ShouldNotBeNull(); // Ensure that the result is not null
			result.Count.ShouldBe(categories.Count); // Assert that the count matches
			result.ShouldContain(c => c.Name == "Beverages"); // Assert that the result contains the correct category
			result.ShouldContain(c => c.Name == "Snacks"); // Assert that the result contains the correct category
		}

		[Test]
		public async Task GetCategoriesAsync_ReturnsEmptyList_WhenNoCategories()
		{
			// Arrange
			var categories = new List<Category>();

			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync())
									.ReturnsAsync(categories);

			// Act
			var result = await _categoryManagementService.GetCategoriesAsync();

			// Assert
			result.ShouldNotBeNull(); 
			result.Count.ShouldBe(0); 
		}

		[Test]
		public async Task GetCategoriesAsync_ReturnsPagedCategories()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "Beverages" }; // Example search term
			var order = "Name"; // Example order by field

			// Mock the data to return a paged response
			var categories = new List<Category>
			{
				new Category { Id = Guid.NewGuid(), Name = "Beverages", Description = "Premium coffee beans." }
			};

			var total = 2; // Total number of categories in the database
			var totalDisplay = 1; // Only one category matches the search criteria

			// Setup the mock to return filtered data
			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository.GetPagedCategoriesAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((categories, total, totalDisplay));

			// Act
			var result = await _categoryManagementService.GetCategoriesAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(categories.Count); // Ensure the correct number of categories is returned
			result.total.ShouldBe(total); // Ensure the total number of categories is correct
			result.totalDisplay.ShouldBe(totalDisplay); // Ensure the total number of displayed categories matches search result

			// Assert that the data contains the expected category
			result.data.ShouldContain(c => c.Name == "Beverages");
		}


		[Test]
		public async Task GetCategoriesAsync_ReturnsEmptyWhenNoCategoriesMatchSearch()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "NonExistentCategory" }; // No category matches this search term
			var order = "Name";

			// Mock the data to return an empty list
			var categories = new List<Category>();
			var total = 0;
			var totalDisplay = 0;

			// Setup the mock to return empty data
			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository.GetPagedCategoriesAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((categories, total, totalDisplay));

			// Act
			var result = await _categoryManagementService.GetCategoriesAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull(); // Ensure the result is not null
			result.data.Count.ShouldBe(0); // Assert that no categories are returned
			result.total.ShouldBe(total); // Ensure the total number of categories is correct
			result.totalDisplay.ShouldBe(totalDisplay); // Ensure the total number of displayed categories is correct
		}

		[Test]
		public async Task GetCategoriesAsync_ReturnsCategoriesWithOrdering()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "" }; // No specific search
			var order = "Name"; // Sorting by category name

			var categories = new List<Category>
			{
				new Category { Id = Guid.NewGuid(), Name = "Beverages", Description = "Premium coffee beans." },
				new Category { Id = Guid.NewGuid(), Name = "Snacks", Description = "Chips and crackers." }
			};

			var total = 2;
			var totalDisplay = 2; // All categories are displayed since no search term is used

			// Setup the mock to return sorted data
			_inventoryUnitOfWorkMock.Setup(x => x.CategoryRepository.GetPagedCategoriesAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((categories, total, totalDisplay));

			// Act
			var result = await _categoryManagementService.GetCategoriesAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(categories.Count); // Ensure the correct number of categories is returned
			result.total.ShouldBe(total); // Ensure the total number of categories is correct
			result.totalDisplay.ShouldBe(totalDisplay); // Ensure the total number of displayed categories is correct

			// Assert that the categories are ordered correctly by Name
			result.data.First().Name.ShouldBe("Beverages");
			result.data.Last().Name.ShouldBe("Snacks");
		}


	}
}