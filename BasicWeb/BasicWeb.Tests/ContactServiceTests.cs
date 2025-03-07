﻿using AutoMapper;
using BasicWeb.Database.Interfaces;
using BasicWeb.Domain;
using BasicWeb.Dto.ContactsDto;
using BasicWeb.Services.Implementations;
using Microsoft.Extensions.Logging;
using Moq;

namespace BasicWeb.Tests
{
    public class ContactServiceTests
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<ILogger<ContactService>> _loggerMock;
        private ContactService _contactService;

        [SetUp]
        public void Init()
        {
            _mapperMock = new Mock<IMapper>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _loggerMock = new Mock<ILogger<ContactService>>();

            _contactService = new(_loggerMock.Object, _contactRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task DeleteContact_ContactNotFound_ThrowsException()
        {
            // Arrange
            int id = 3;
            _contactRepositoryMock.Setup(x => x.GetById(id)).Returns((Contact)null);

            // Act & Assert
            Assert.Throws<Exception>(() => _contactService.DeleteContact(id));
            _loggerMock.Verify(
                        x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Delete failed: Contact with id: {id} doesnt exists")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                        Times.Once);
        }

        [Test]
        public async Task DeleteContact_SuccessfulyDeleted()
        {
            // Arrange
            int id = 3;
            string name = "Test";

            Contact contact = new Contact()
            {
                Id = id,
                CompanyId = 1,
                CountryId = 1,
                Name = name
            };

            _contactRepositoryMock.Setup(x => x.GetById(id)).Returns(contact);

            // Act 
            _contactService.DeleteContact(id);

            // Assert
            _contactRepositoryMock.Verify(x => x.Delete(It.Is<Contact>(y => y.Id == id && y.Name == name)), Times.Once);
        }

        [Test]
        public async Task UpdateContact_ContactNotFound_ThrowsException()
        {
            // Arrange
            int id = 5;
            string name = "Test";
            UpdateContactDto contact = new UpdateContactDto()
            {
                Id = id,
                Name = name
            };
            _contactRepositoryMock.Setup(x => x.GetById(id)).Returns((Contact)null);

            // Act & Assert
            Assert.Throws<Exception>(() => _contactService.UpdateContact(contact));
            _loggerMock.Verify(
                        x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Update failed: A contact was not found")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                        Times.Once);
        }

        [Test]
        public async Task UpdateContact_UpdateSuccessfuly()
        {
            // Arrange
            int id = 5;
            string name = "Test";
            UpdateContactDto contactDto = new UpdateContactDto()
            {
                Id = id,
                Name = name
            };

            Contact contact = new Contact()
            {
                Id = id,
                Name = name,
            };

            _contactRepositoryMock.Setup(x => x.GetById(id)).Returns(contact);

            // Act
            _contactService.UpdateContact(contactDto);

            // Assert
            _contactRepositoryMock.Verify(x => x.Update(It.Is<Contact>(x => x.Id == id)), Times.Once);
        }
    }
}
