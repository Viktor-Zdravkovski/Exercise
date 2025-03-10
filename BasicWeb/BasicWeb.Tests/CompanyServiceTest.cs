using AutoMapper;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using BasicWeb.Dto.CompanyDto;
using BasicWeb.Dto.ContactsDto;
using BasicWeb.Services.Implementations;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BasicWeb.Tests
{
    public class CompanyServiceTest
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IRepository<Company>> _companyRepositoryMock;
        private Mock<ILogger<CompanyService>> _loggerMock;
        private CompanyService _companyService;

        [SetUp]
        public void Init()
        {
            _mapperMock = new Mock<IMapper>();
            _companyRepositoryMock = new Mock<IRepository<Company>>();
            _loggerMock = new Mock<ILogger<CompanyService>>();

            _companyService = new(_loggerMock.Object, _companyRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task DeleteCompany_CompanyNotFound_ThrowsException()
        {
            // Arrange
            int id = 15;
            _companyRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync((Company)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(() => _companyService.DeleteCompany(id));

            Assert.AreEqual($"Company with ID: {id} was not found", exception.Message);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Delete failed: Company with id: {id} doesnt exists")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task DeleteCompany_SuccessfulyDeleted()
        {
            // Arrange
            int id = 7;
            string name = "TestDelete";

            Company contact = new Company()
            {
                Id = id,
                Name = name
            };

            _companyRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(contact);

            // Act 
            await _companyService.DeleteCompany(id);

            // Assert
            _companyRepositoryMock.Verify(x => x.Delete(It.Is<Company>(y => y.Id == id && y.Name == name)), Times.Once);
        }

        [Test]
        public async Task UpdateCompany_CompanyNotFound_ThrowsException()
        {
            // Arrange
            int id = 5;
            string name = "TestUpdate";
            UpdateCompanyDto contact = new UpdateCompanyDto()
            {
                Id = id,
                Name = name
            };

            _companyRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync((Company)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await _companyService.UpdateCompany(contact));

            Assert.AreEqual("No company found to update", exception.Message);

            _loggerMock.Verify(
                        x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Update Failed: A company was not found")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                        Times.Once);
        }

        [Test]
        public void UpdateCompany_NameIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var company = new UpdateCompanyDto
            {
                Id = 5,
                Name = ""
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _companyService.UpdateCompany(company));
            Assert.AreEqual("Company name cannot be empty", exception.Message);
        }

        [Test]
        public async Task UpdateCompany_UpdateSuccessfuly()
        {
            // Arrange
            int id = 5;
            string name = "TestUpdates";
            UpdateCompanyDto contactDto = new UpdateCompanyDto()
            {
                Id = id,
                Name = name
            };

            Company contact = new Company()
            {
                Id = id,
                Name = name,
            };

            _companyRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(contact);

            // Act
            _companyService.UpdateCompany(contactDto);

            // Assert
            _companyRepositoryMock.Verify(x => x.Update(It.Is<Company>(x => x.Id == id)), Times.Once);
        }

        [Test]
        public async Task Getting_All_Companies_Successfuly()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company { Id = 1, Name = "Company 1" },
                new Company { Id = 2, Name = "Company 2" }
            };

            var companyDtos = new List<CompanyDto>
            {
                new CompanyDto { Id = 1, Name = "Company 1" },
                new CompanyDto { Id = 2, Name = "Company 2" }
            };

            _companyRepositoryMock.Setup(x => x.Get()).ReturnsAsync(companies);

            _mapperMock.Setup(x => x.Map<List<CompanyDto>>(companies)).Returns(companyDtos);

            // Act
            var result = await _companyService.GetAll();

            // Assert
            Assert.AreEqual(companyDtos, result);

            _companyRepositoryMock.Verify(x => x.Get(), Times.Once);
            _mapperMock.Verify(x => x.Map<List<CompanyDto>>(companies), Times.Once);
        }

        [Test]
        public async Task Getting_All_Companies_Unsuccessfuly()
        {
            var emptyList = new List<Company>();
            _companyRepositoryMock.Setup(x => x.Get()).ReturnsAsync(emptyList);

            // Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(() => _companyService.GetAll());

            Assert.AreEqual("No companies found", exception.Message);

            _companyRepositoryMock.Verify(x => x.Get(), Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No companies found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
