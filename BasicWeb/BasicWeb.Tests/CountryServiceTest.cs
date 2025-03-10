using AutoMapper;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using BasicWeb.Dto.CompanyDto;
using BasicWeb.Dto.ContactsDto;
using BasicWeb.Dto.CountryDto;
using BasicWeb.Services.Implementations;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BasicWeb.Tests
{
    public class CountryServiceTest
    {
        private Mock<IMapper> _mapperMock;
        private Mock<ICountryRepository> _countryRepositoryMock;
        private Mock<ILogger<CountryService>> _loggerMock;
        private CountryService _countryService;

        [SetUp]
        public void Init()
        {
            _mapperMock = new Mock<IMapper>();
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _loggerMock = new Mock<ILogger<CountryService>>();

            _countryService = new(_loggerMock.Object, _countryRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task DeleteCountry_CountryNotFound_ThrowsException()
        {
            // Arrange
            int id = 13;
            _countryRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync((Country)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(() => _countryService.DeleteCountry(id));

            Assert.AreEqual($"Country with ID: {id} was not found", exception.Message);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Delete failed: Country with id: {id} doesnt exists")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task DeleteCountry_SuccessfulyDeleted()
        {
            // Arrange
            int id = 3;

            Country contact = new Country()
            {
                Id = id
            };

            _countryRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(contact);

            // Act 
            await _countryService.DeleteCountry(id);

            // Assert
            _countryRepositoryMock.Verify(x => x.Delete(It.Is<Country>(y => y.Id == id)), Times.Once);
        }

        [Test]
        public async Task UpdateCountry_CountryNotFound_ThrowsException()
        {
            // Arrange
            int id = 5;
            string name = "Test";
            UpdateCountryDto contact = new UpdateCountryDto()
            {
                Id = id,
                Name = name
            };
            _countryRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync((Country)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await _countryService.UpdateCountry(contact));

            Assert.AreEqual("No country found to update", exception.Message);

            _loggerMock.Verify(
                        x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Update Failed: A country was not found")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                        Times.Once);
        }

        [Test]
        public void UpdateCountry_NameIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var contact = new UpdateCountryDto
            {
                Id = 5,
                Name = ""
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _countryService.UpdateCountry(contact));
            Assert.AreEqual("Country name cannot be empty", exception.Message);
        }

        [Test]
        public async Task UpdateCountry_UpdateSuccessfuly()
        {
            // Arrange
            int id = 5;
            string name = "Test";
            UpdateCountryDto contactDto = new UpdateCountryDto()
            {
                Id = id,
                Name = name
            };

            Country contact = new Country()
            {
                Id = id,
                Name = name,
            };

            _countryRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(contact);

            // Act
            _countryService.UpdateCountry(contactDto);

            // Assert
            _countryRepositoryMock.Verify(x => x.Update(It.Is<Country>(x => x.Id == id)), Times.Once);
        }
    }
}
