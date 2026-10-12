using Application;
using Application.Services;
using Domain.Entities.Things;
using Domain.Entities.Transport;
using Domain.Services;
using Moq;
using Xunit;

namespace VyshKat.Tests
{
    public class ParkServiceTests
    {
        [Fact]
        public void AddTransport_WhenInspectionPasses_ShouldReturnTrueAndAddTransport()
        {
            var mockServiceCenter = new Mock<IServiceCenter>();
            mockServiceCenter
                .Setup(sc => sc.Inspect(It.IsAny<ITransport>()))
                .Returns(true);

            var parkService = new ParkService(mockServiceCenter.Object);
            var scooter = new ElectricScooter("SC-001", 8, 1.5);

            var result = parkService.AddTransport(scooter);

            Assert.True(result);
            Assert.Contains(scooter.InventoryId, parkService.GetTransportIds());
        }
        [Fact]
        public void AddTransport_WhenInspectionFails_ShouldReturnFalse()
        {
            var mockServiceCenter = new Mock<IServiceCenter>();
            mockServiceCenter.Setup(sc => sc.Inspect(It.IsAny<ITransport>())).Returns(false);

            var parkService = new ParkService(mockServiceCenter.Object);
            var scooter = new ElectricScooter("SC-1", 7, 1.5);

            var result = parkService.AddTransport(scooter);

            Assert.False(result);
        }
        [Fact]
        public void GetTransportsForNovices_ShouldReturnOnlyTransportsWithFriendlinessGreaterOrEqualSix()
        {
            var mockServiceCenter = new Mock<IServiceCenter>();
            mockServiceCenter
                .Setup(sc => sc.Inspect(It.IsAny<ITransport>()))
                .Returns(true);

            var parkService = new ParkService(mockServiceCenter.Object);

            var easyScooter = new ElectricScooter("EASY-1", 8, 1.0);
            var hardBicycle = new Bicycle("HARD-1", 3);
            var borderCase = new Bicycle("BORD-1", 6);

            parkService.AddTransport(easyScooter);
            parkService.AddTransport(hardBicycle);
            parkService.AddTransport(borderCase);

            var result = parkService.GetTransportsForNovices();

            Assert.Equal(2, result.Count);
            Assert.Contains(easyScooter, result);
            Assert.Contains(borderCase, result);
            Assert.DoesNotContain(hardBicycle, result);
        }
        [Fact]
        public void GetTotalDailyEnergy_ShouldSumEnergyFromTransportsAndThings()
        {
            var mockServiceCenter = new Mock<IServiceCenter>();
            mockServiceCenter.Setup(sc => sc.Inspect(It.IsAny<ITransport>())).Returns(true);
            mockServiceCenter.Setup(sc => sc.Inspect(It.IsAny<IThing>())).Returns(true);

            var parkService = new ParkService(mockServiceCenter.Object);

            parkService.AddTransport(new ElectricScooter("SC-1", 8, 1.5));
            parkService.AddTransport(new Bicycle("BK-1", 5));
            parkService.AddThing(new ChargingStation("CS-1", 2.0)); 

            var total = parkService.GetTotalDailyEnergy();

            Assert.Equal(3.5, total, precision: 2);
        }
        
    }
    public class ElectricScooterTests
    {
        [Theory]
        [InlineData("", 5, 1.0)]                  // Пустой ID
        [InlineData("   ", 5, 1.0)]               // Пробелы
        [InlineData("SC-1", 0, 1.0)]              // friendliness < 1
        [InlineData("SC-1", 11, 1.0)]             // friendliness > 10
        public void Constructor_WithInvalidData_ShouldThrow(string id, int friendliness, double energy)
        {
            Assert.Throws<ArgumentException>(() => new ElectricScooter(id, friendliness, energy));
        }

        [Fact]
        public void Constructor_WithValidData_ShouldCreateObject()
        {
            var scooter = new ElectricScooter("SC-1", 7, 1.5);
            Assert.Equal("SC-1", scooter.InventoryId);
            Assert.Equal(7, scooter.NoviceFriendliness);
            Assert.Equal(1.5, scooter.DailyEnergyConsumption);
        }
    }
    public class ScooterTests
    {
        [Theory]
        [InlineData("", 5)]                  // Пустой ID
        [InlineData("   ", 5)]               // Пробелы
        [InlineData("SC-1", 0)]              // friendliness < 1
        [InlineData("SC-1", 11)]             // friendliness > 10
        public void Constructor_WithInvalidData_ShouldThrow(string id, int friendliness)
        {
            Assert.Throws<ArgumentException>(() => new Scooter(id, friendliness));
        }

        [Fact]
        public void Constructor_WithValidData_ShouldCreateObject()
        {
            var scooter = new Scooter("SC-1", 7);
            Assert.Equal("SC-1", scooter.InventoryId);
            Assert.Equal(7, scooter.NoviceFriendliness);
        }
    }
    public class ElectriИшснсдуTests
    {
        [Theory]
        [InlineData("", 5, 1.0)]                  // Пустой ID
        [InlineData("   ", 5, 1.0)]               // Пробелы
        [InlineData("SC-1", 0, 1.0)]              // friendliness < 1
        [InlineData("SC-1", 11, 1.0)]             // friendliness > 10
        public void Constructor_WithInvalidData_ShouldThrow(string id, int friendliness, double energy)
        {
            Assert.Throws<ArgumentException>(() => new ElectricBicycle(id, friendliness, energy));
        }

        [Fact]
        public void Constructor_WithValidData_ShouldCreateObject()
        {
            var bicycle = new ElectricBicycle("SC-1", 7, 1.5);
            Assert.Equal("SC-1", bicycle.InventoryId);
            Assert.Equal(7, bicycle.NoviceFriendliness);
            Assert.Equal(1.5, bicycle.DailyEnergyConsumption);
        }
    }
    public class BicycleTests
    {
        [Theory]
        [InlineData("", 5)]                  // Пустой ID
        [InlineData("   ", 5)]               // Пробелы
        [InlineData("SC-1", 0)]              // friendliness < 1
        [InlineData("SC-1", 11)]             // friendliness > 10
        public void Constructor_WithInvalidData_ShouldThrow(string id, int friendliness)
        {
            Assert.Throws<ArgumentException>(() => new Bicycle(id, friendliness));
        }

        [Fact]
        public void Constructor_WithValidData_ShouldCreateObject()
        {
            var bicycle = new Bicycle("SC-1", 7);
            Assert.Equal("SC-1", bicycle.InventoryId);
            Assert.Equal(7, bicycle.NoviceFriendliness);
        }
    }
    public class HelmetTests
    {
        [Theory]
        [InlineData("")]                      // Пустой ID
        public void Constructor_WithInvalidData_ShouldThrow(string id)
        {
            Assert.Throws<ArgumentException>(() => new Helmet(id));
        }
    }
    public class ChargingStationTests
    {
        [Theory]
        [InlineData("", 5)]                  // Пустой ID
        public void Constructor_WithInvalidData_ShouldThrow(string id, int energy)
        {
            Assert.Throws<ArgumentException>(() => new ChargingStation(id, energy));
        }

        [Fact]
        public void Constructor_WithValidData_ShouldCreateObject()
        {
            var station = new ChargingStation("SC-1", 7);
            Assert.Equal("SC-1", station.InventoryId);
            Assert.Equal(7, station.DailyEnergyConsumption);
        }
    }
}
