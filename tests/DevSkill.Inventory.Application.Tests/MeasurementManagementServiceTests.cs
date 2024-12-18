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
	public class MeasurementManagementServiceTests
	{
		private AutoMock _moq;
		private IMeasurementManagementService _measurementManagementService;
		private Mock<IInventoryUnitOfWork> _inventoryUnitOfWorkMock;
		private Mock<IMeasurementRepository> _measurementRepositoryMock;

		[SetUp]
		public void Setup()
		{
			_measurementManagementService = _moq.Create<MeasurementManagementService>();
			_inventoryUnitOfWorkMock = _moq.Mock<IInventoryUnitOfWork>();
			_measurementRepositoryMock = _moq.Mock<IMeasurementRepository>();
		}

		[TearDown]
		public void Teardown()
		{
			_inventoryUnitOfWorkMock?.Reset();
			_measurementRepositoryMock?.Reset();
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
		public async Task CreateMeasurementUnitAsync_TitleNotDuplicate_CreatesMeasurementUnit()
		{
			// Arrange
			var measurement = new Measurement
			{
				MeasurementName = "Kilogram",
				Symbol = "Kg",
				Items = new List<Item>()
			};

			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository).Returns(_measurementRepositoryMock.Object);
			_measurementRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(measurement.MeasurementName, null)).ReturnsAsync(false);
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Verifiable();
			_measurementRepositoryMock.Setup(x => x.AddAsync(measurement)).Verifiable();

			//Act
			await _measurementManagementService.CreateMeasurementUnitAsync(measurement);

			// Assert
			_measurementRepositoryMock.VerifyAll();
			_inventoryUnitOfWorkMock.VerifyAll();
		}

		[Test]
		public async Task UpdateMeasurmentUnitAsync_DuplicateTitle_ThrowsException()
		{
			// Arrange
			var measurement = new Measurement
			{
				Id = Guid.NewGuid(),
				MeasurementName = "Kilogram",
				Symbol = "Kg",
				Items = new List<Item>()
			};

			var error = "Title Should be Unique";

			// Setup mock repository
			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository).Returns(_measurementRepositoryMock.Object);
			_measurementRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(measurement.MeasurementName, measurement.Id)).ReturnsAsync(true);

			// Act & Assert
			var exception = await Should.ThrowAsync<InvalidOperationException>(() => _measurementManagementService.UpdateMeasurmentUnitAsync(measurement));

			// Assert
			exception.Message.ShouldBe(error);
		}


		[Test]
		public async Task DeleteMeasurementUnitAsync_ValidMeasurementId_DeletesMeasurement()
		{
			// Arrange
			var measurementId = Guid.NewGuid();

			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository).Returns(_measurementRepositoryMock.Object);
			_measurementRepositoryMock.Setup(x => x.RemoveAsync(measurementId)).Returns(Task.CompletedTask); // Simulate async remove
			_inventoryUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask); // Simulate save after removal

			// Act
			await _measurementManagementService.DeleteMeasurementUnitAsync(measurementId);

			// Assert
			_measurementRepositoryMock.Verify(x => x.RemoveAsync(measurementId));
			_inventoryUnitOfWorkMock.Verify(x => x.SaveAsync());
		}

		[Test]
		public async Task GetMeasurementUnitAsync_ValidMeasurementId_ReturnsMeasurementUnit()
		{
			// Arrange
			var measurementId = Guid.NewGuid(); 
			var expectedMeasurementUnit = new Measurement
			{
				Id = measurementId,
				MeasurementName = "Kilogram",
				Symbol = "Kg",
				Items = new List<Item>()
			};


			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository.GetByIdAsync(measurementId))
									.ReturnsAsync(expectedMeasurementUnit);

			// Act
			var result = await _measurementManagementService.GetMeasurementUnitAsync(measurementId);

			// Assert
			result.ShouldNotBeNull();
			result.Id.ShouldBe(expectedMeasurementUnit.Id);
			result.MeasurementName.ShouldBe(expectedMeasurementUnit.MeasurementName);
			result.Symbol.ShouldBe(expectedMeasurementUnit.Symbol);
		}

		[Test]
		public async Task GetMeasurementUnitsAsync_ReturnsListOfMeasurementUnits()
		{
			// Arrange
			var measurementUnits = new List<Measurement>
			{
				new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram", Symbol = "Kg" },
				new Measurement { Id = Guid.NewGuid(), MeasurementName = "Liter", Symbol = "L" },
			};

			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository.GetAllAsync())
									.ReturnsAsync(measurementUnits);

			// Act
			var result = await _measurementManagementService.GetMeasurementUnitsAsync();

			// Assert
			result.ShouldNotBeNull(); 
			result.Count.ShouldBe(measurementUnits.Count); 
			result.ShouldContain(c => c.MeasurementName == "Kilogram"); 
			result.ShouldContain(c => c.MeasurementName == "Liter"); 
		}

		[Test]
		public async Task GetMeasurementUnitsAsync_ReturnsEmptyList_WhenNoMeasurementUnits()
		{
			// Arrange
			var measurementUnits = new List<Measurement>();

			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository.GetAllAsync())
									.ReturnsAsync(measurementUnits);

			// Act
			var result = await _measurementManagementService.GetMeasurementUnitsAsync();

			// Assert
			result.ShouldNotBeNull();
			result.Count.ShouldBe(0);
		}

		[Test]
		public async Task GetMeasurementUnitsAsync_ReturnsPagedMeasurementUnits()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "Kilogram" }; 
			var order = "MeasurementName"; 

			var measurementunits = new List<Measurement>
			{
				new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram", Symbol = "Kg" }
			};

			var total = 2; 
			var totalDisplay = 1;

			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository.GetPagedMeasurementUnitsAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((measurementunits, total, totalDisplay));

			// Act
			var result = await _measurementManagementService.GetMeasurementUnitsAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(measurementunits.Count); 
			result.total.ShouldBe(total);
			result.totalDisplay.ShouldBe(totalDisplay); 

			result.data.ShouldContain(c => c.MeasurementName == "Kilogram");
		}


		[Test]
		public async Task GetMeasurementUnitsAsync_ReturnsEmptyWhenNoMeasurementUnitsMatchSearch()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "NonExistentMeasurement" }; 
			var order = "Name";

			// Mock the data to return an empty list
			var measurementUnits = new List<Measurement>();
			var total = 0;
			var totalDisplay = 0;

			// Setup the mock to return empty data
			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository.GetPagedMeasurementUnitsAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((measurementUnits, total, totalDisplay));

			// Act
			var result = await _measurementManagementService.GetMeasurementUnitsAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull(); 
			result.data.Count.ShouldBe(0); 
			result.total.ShouldBe(total); 
			result.totalDisplay.ShouldBe(totalDisplay); 
		}

		[Test]
		public async Task GetMeasurementUnitsAsync_ReturnsMeasurementUnitsWithOrdering()
		{
			// Arrange
			var pageIndex = 1;
			var pageSize = 2;
			var search = new DataTablesSearch { Value = "" }; // No specific search
			var order = "MeasurementName";

			var measurementunits = new List<Measurement>
			{
				new Measurement { Id = Guid.NewGuid(), MeasurementName = "Kilogram", Symbol = "Kg" },
				new Measurement { Id = Guid.NewGuid(), MeasurementName = "Liter", Symbol = "L" }
			};

			var total = 2;
			var totalDisplay = 2; 

			// Setup the mock to return sorted data
			_inventoryUnitOfWorkMock.Setup(x => x.MeasurementRepository.GetPagedMeasurementUnitsAsync(pageIndex, pageSize, search, order))
									.ReturnsAsync((measurementunits, total, totalDisplay));

			// Act
			var result = await _measurementManagementService.GetMeasurementUnitsAsync(pageIndex, pageSize, search, order);

			// Assert
			result.data.ShouldNotBeNull();
			result.data.Count.ShouldBe(measurementunits.Count); // Ensure the correct number of categories is returned
			result.total.ShouldBe(total); // Ensure the total number of categories is correct
			result.totalDisplay.ShouldBe(totalDisplay); // Ensure the total number of displayed categories is correct

			// Assert that the categories are ordered correctly by Name
			result.data.First().MeasurementName.ShouldBe("Kilogram");
			result.data.Last().MeasurementName.ShouldBe("Liter");
		}


	}
}
