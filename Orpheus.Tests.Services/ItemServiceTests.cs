using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MockQueryable.Moq;
using NUnit.Framework;
using Orpheus.Core.Implementations;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class ItemServiceTests
    {
        private Mock<IRepository<Item, Guid>> mockItemRepo = null!;
        private ItemService service = null!;

        [SetUp]
        public void Setup()
        {
            mockItemRepo = new Mock<IRepository<Item, Guid>>();
            service = new ItemService(mockItemRepo.Object);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnItem_WhenAvailable()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new Item
            {
                Id = id,
                IsAvailable = true,
                Name = "Test Item"
            };
            var items = new List<Item> { item }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(items.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result!.Id);
            Assert.IsTrue(result.IsAvailable);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenItemNotAvailable()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new Item
            {
                Id = id,
                IsAvailable = false
            };
            var items = new List<Item> { item }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(items.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenItemNotFound()
        {
            // Arrange
            var items = new List<Item>().AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(items.Object);

            // Act
            var result = await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAvailableItemsAsync_ShouldReturnOnlyAvailableItems()
        {
            // Arrange
            var availableItem = new Item { Id = Guid.NewGuid(), IsAvailable = true };
            var unavailableItem = new Item { Id = Guid.NewGuid(), IsAvailable = false };

            var items = new List<Item> { availableItem, unavailableItem }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(items.Object);

            // Act
            var result = await service.GetAvailableItemsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.All(i => i.IsAvailable));
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(availableItem.Id));
        }
    }
}