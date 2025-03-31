using Xunit;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using DealershipSystem.Services;
using DealershipSystem.Context;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DealershipSystem.Tests
{
    public class LocationServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LocationService _service;

        public LocationServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _service = new LocationService(_mockMapper.Object, _context);
        }

        // Ensuring the database is cleared before each test
        private async Task ClearDatabaseAsync()
        {
            foreach (var entity in _context.Set<Location>())
            {
                _context.Remove(entity);
            }
            foreach (var entity in _context.Set<Address>())
            {
                _context.Remove(entity);
            }
            foreach (var entity in _context.Set<Prefecture>())
            {
                _context.Remove(entity);
            }
            await _context.SaveChangesAsync();
        }


        [Fact]
        public async Task GetLocationByIdAsync_Returns_LocationDto_When_Found()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            var locationId = 1;
            var location = new Location
            {
                ID = locationId,
                LocationName = "テスト場所", // Non-Romanized
                AddressId = 1,
                MaxCapacity = 10,
                PhoneNumber = "1234567890"
            };

            var address = new Address
            {
                Id = 1,
                PostalCode = "123-4567",
                Prefecture = new Prefecture { Name = "Tokyo", NameJP = "東京都" }, // Non-Romanized
                City = "渋谷", // Non-Romanized
                CityRomanized = "Shibuya", // Romanized
                Street = "1-1-1 渋谷", // Non-Romanized
                StreetRomanized = "1-1-1 Shibuya" // Romanized
            };

            _context.Addresses.Add(address);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            var locationDto = new LocationDto
            {
                Id = locationId,
                LocationName = "テスト場所", // Non-Romanized
                Address = new AddressDto
                {
                    PostalCode = "123-4567",
                    Prefecture = new PrefectureDTO {  Name = "Tokyo", NameJP = "東京都" }, // Non-Romanized
                    City = "渋谷", // Non-Romanized
                    CityRomanized = "Shibuya", // Romanized
                    Street = "1-1-1 渋谷", // Non-Romanized
                    StreetRomanized = "1-1-1 Shibuya" // Romanized
                },
                MaxCapacity = 10,
                PhoneNumber = "1234567890"
            };

            _mockMapper.Setup(m => m.Map<LocationDto>(It.IsAny<Location>())).Returns(locationDto);

            var result = await _service.GetLocationByIdAsync(locationId);

            Assert.NotNull(result);
            Assert.Equal(locationId, result.Id);
            Assert.Equal("テスト場所", result.LocationName); // Non-Romanized
            Assert.Equal("123-4567", result.Address.PostalCode);
            Assert.Equal("Tokyo", result.Address.Prefecture.Name); // Non-Romanized
            Assert.Equal("東京都", result.Address.Prefecture.NameJP); // Non-Romanized
            Assert.Equal("渋谷", result.Address.City); // Non-Romanized
            Assert.Equal("Shibuya", result.Address.CityRomanized); // Romanized
            Assert.Equal("1-1-1 渋谷", result.Address.Street); // Non-Romanized
            Assert.Equal("1-1-1 Shibuya", result.Address.StreetRomanized); // Romanized
            Assert.Equal("1234567890", result.PhoneNumber);
        }

        [Fact]
        public async Task GetLocationByIdAsync_Returns_Null_When_Not_Found_MinusID()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            var result = await _service.GetLocationByIdAsync(-100);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllLocationsAsync_Returns_List_Of_Locations()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            var locations = new List<Location>
            {
                new Location { ID = 1, LocationName = "テスト場所1", AddressId = 1, MaxCapacity = 10, PhoneNumber = "1234567890" },
                new Location { ID = 2, LocationName = "テスト場所2", AddressId = 2, MaxCapacity = 20, PhoneNumber = "0987654321" }
            };
            var address1 = new Address
            {
                Id = 1,
                PostalCode = "123-4567",
                Prefecture = new Prefecture { Name = "東京都", NameJP = "Tokyo" },
                City = "渋谷",
                CityRomanized = "Shibuya",
                Street = "1-1-1 渋谷",
                StreetRomanized = "1-1-1 Shibuya"
            };
            var address2 = new Address
            {
                Id = 2,
                PostalCode = "234-5678",
                Prefecture = new Prefecture { Name = "大阪府", NameJP = "Osaka" },
                City = "梅田",
                CityRomanized = "Umeda",
                Street = "2-2-2 梅田",
                StreetRomanized = "2-2-2 Umeda"
            };

            _context.Addresses.Add(address1);
            _context.Addresses.Add(address2);
            _context.Locations.AddRange(locations);
            await _context.SaveChangesAsync();

            var result = await _service.GetAllLocationsAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("テスト場所1", result[0].LocationName); // Non-Romanized
            Assert.Equal("テスト場所2", result[1].LocationName); // Non-Romanized
        }

        [Fact]
        public async Task DeleteLocationAsync_Returns_True_When_Deleted()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            var location = new Location { ID = 1, LocationName = "テスト場所", AddressId = 1, MaxCapacity = 10, PhoneNumber = "1234567890" };
            var address = new Address
            {
                Id = 1,
                PostalCode = "123-4567",
                Prefecture = new Prefecture { Name = "東京都", NameJP = "Tokyo" },
                City = "渋谷",
                CityRomanized = "Shibuya",
                Street = "1-1-1 渋谷",
                StreetRomanized = "1-1-1 Shibuya"
            };

            _context.Addresses.Add(address);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteLocationAsync(1);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteLocationAsync_Returns_False_When_Not_Found()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            var result = await _service.DeleteLocationAsync(1);
            Assert.False(result);
        }

        // Parameterized test with unique location information
        [Theory]
        [InlineData(1, "テスト場所1", "1-1-1 渋谷", "渋谷", "Shibuya", "Shibuya", 10, "1234567890")]
        [InlineData(2, "テスト場所2", "2-2-2 大阪", "大阪", "Umeda", "Umeda", 20, "9876543210")]
        public async Task GetLocationByIdAsync_Handles_Multiple_Cases(int locationId, string locationName, string street, string city, string cityRomanized, string streetRomanized, int maxCapacity, string phoneNumber)
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            var location = new Location
            {
                ID = locationId,
                LocationName = locationName,
                AddressId = 1,
                MaxCapacity = maxCapacity,
                PhoneNumber = phoneNumber
            };

            var prefecture = new Prefecture
            {
                Name = "東京都",
                NameJP = "Tokyo"
            };
            
            var address = new Address
            {
                Id = 1,
                PostalCode = "123-4567",
                Prefecture = new Prefecture { Name = "東京都", NameJP = "Tokyo" },
                City = city,
                CityRomanized = cityRomanized,
                Street = street,
                StreetRomanized = streetRomanized
            };

            _context.Addresses.Add(address);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            var locationDto = new LocationDto
            {
                Id = locationId,
                LocationName = locationName,
                Address = new AddressDto
                {
                    PostalCode = "123-4567",
                    Prefecture = new PrefectureDTO { Name = "東京都", NameJP = "Tokyo" },
                    City = city,
                    CityRomanized = cityRomanized,
                    Street = street,
                    StreetRomanized = streetRomanized
                },
                MaxCapacity = maxCapacity,
                PhoneNumber = phoneNumber
            };

            _mockMapper.Setup(m => m.Map<LocationDto>(It.IsAny<Location>())).Returns(locationDto);

            var result = await _service.GetLocationByIdAsync(locationId);

            Assert.NotNull(result);
            Assert.Equal(locationId, result.Id);
            Assert.Equal(locationName, result.LocationName);
            Assert.Equal(street, result.Address.Street);
            Assert.Equal(city, result.Address.City);
            Assert.Equal(cityRomanized, result.Address.CityRomanized);
            Assert.Equal(streetRomanized, result.Address.StreetRomanized);
            Assert.Equal(maxCapacity, result.MaxCapacity);
            Assert.Equal(phoneNumber, result.PhoneNumber);
        }
        
        [Fact]
        public async Task UpdateLocationAsync_ReturnsNull_WhenLocationNotFound()
        {
            // Arrange
            var invalidLocationId = 999; // Assuming this ID does not exist in the database

            // Create a valid prefecture for this test
            var prefecture = new Prefecture
            {
                Name = "東京都", // Name in English
                NameJP = "Tokyo" // Name in Japanese
            };
            _context.Prefectures.Add(prefecture);
            await _context.SaveChangesAsync();

            // Create the location DTO for the update
            var locationDto = new LocationDto
            {
                Id = invalidLocationId, // Provide an invalid ID
                LocationName = "Updated Location",
                Address = new AddressDto
                {
                    PostalCode = "123-4567",
                    City = "Updated City",
                    CityRomanized = "Updated City Romanized",
                    Street = "Updated Street",
                    StreetRomanized = "Updated Street Romanized",
                    Prefecture = new PrefectureDTO { Name = "Tokyo", NameJP = "東京都" }
                },
                MaxCapacity = 200,
                PhoneNumber = "9876543210"
            };

            // Act
            var result = await _service.UpdateLocationAsync(locationDto);

            // Assert
            Assert.Null(result); // Assert that the result is null (indicating no location was found with the provided ID)
        }

        [Fact]
        public async Task GetLocationByIdAsync_Returns_Null_When_Location_Not_Found()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            var result = await _service.GetLocationByIdAsync(99); // Non-existent ID
            Assert.Null(result); // Should return null as location doesn't exist
        }
        
        [Fact]
        public async Task CreateLocationAsync_Returns_Null_When_Prefecture_Not_Found()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            // Adding required prefectures to the database
            var prefectures = new List<Prefecture>
            {
                new Prefecture { Name = "Tokyo", NameJP = "東京都" },
                new Prefecture { Name = "Osaka", NameJP = "大阪府" }
            };

            _context.Prefectures.AddRange(prefectures);
            await _context.SaveChangesAsync();
            
            var locationDto = new LocationDto
            {
                Id = 1,
                LocationName = "New Location",
                Address = new AddressDto
                {
                    PostalCode = "123-4567",
                    Prefecture = new PrefectureDTO { Name = "NonExistentPrefecture", NameJP = "不存在的都道府県" }, // Prefecture does not exist
                    City = "New City",
                    CityRomanized = "New City Romanized",
                    Street = "New Street",
                    StreetRomanized = "New Street Romanized"
                },
                MaxCapacity = 100,
                PhoneNumber = "1234567890"
            };

            var result = await _service.CreateLocationAsync(locationDto);

            Assert.Null(result); // Check if the result is null
        }

        [Fact]
        public async Task GetCarUsageInLocationAsync_Returns_Correct_Usage()
        {
            await ClearDatabaseAsync();  // Ensure database is clear before the test

            // Prepare test data
            var location = new Location
            {
                ID = 1,
                LocationName = "Test Location",
                AddressId = 1,
                MaxCapacity = 10,
                PhoneNumber = "1234567890"
            };

            var address = new Address
            {
                Id = 1,
                PostalCode = "123-4567",
                Prefecture = new Prefecture { Name = "Tokyo", NameJP = "東京都" },
                City = "Shibuya",
                CityRomanized = "Shibuya",
                Street = "1-1-1 Shibuya",
                StreetRomanized = "1-1-1 Shibuya"
            };

            var cars = new List<Car>
            {
                new Car { ID = 1, LocationID = 1 },
                new Car { ID = 2, LocationID = 1 },
                new Car { ID = 3, LocationID = 1 }
            };

            _context.Addresses.Add(address);
            _context.Locations.Add(location);
            _context.Cars.AddRange(cars);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetCarUsageInLocationAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result?.MaxCapacity);
            Assert.Equal(3, result?.CurrentUsage); // There are 3 cars in the location
        }
    }
}