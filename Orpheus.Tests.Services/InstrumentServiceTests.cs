using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MockQueryable.Moq;
using NUnit.Framework;
using Orpheus.Core.DTOs;
using Orpheus.Core.Implementations;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class InstrumentServiceTests
    {
        private Mock<IRepository<Item, Guid>> mockItemRepo = null!;
        private Mock<IRepository<ItemImage, Guid>> mockItemImageRepo = null!;
        private InstrumentService service = null!;

        [SetUp]
        public void Setup()
        {
            mockItemRepo = new Mock<IRepository<Item, Guid>>();
            mockItemImageRepo = new Mock<IRepository<ItemImage, Guid>>();
            service = new InstrumentService(mockItemRepo.Object, mockItemImageRepo.Object);
        }

        [Test]
        public async Task GetAvailableInstrumentsAsync_ShouldReturnOnlyAvailableInstruments()
        {
            // Arrange
            var instruments = new List<Item>
            {
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Instrument, IsAvailable = true },
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Instrument, IsAvailable = false },
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Album, IsAvailable = true }
            }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(instruments.Object);

            // Act
            var result = await service.GetAvailableInstrumentsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().ItemType, Is.EqualTo(ItemType.Instrument));
            Assert.That(result.First().IsAvailable, Is.True);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            var id = Guid.NewGuid();
            var instrument = new Item { Id = id, Name = "Electric Guitar", ItemType = ItemType.Instrument };
            var items = new List<Item> { instrument }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(items.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Electric Guitar"));
        }

        [Test]
        public async Task GetFeaturedInstrumentsAsync_ShouldReturnLimitedInstrumentsOrderedByReviewsCount()
        {
            // Arrange
            var instrument1Id = Guid.NewGuid();
            var instrument2Id = Guid.NewGuid();
            var instruments = new List<Item>
            {
                new Item
                {
                    Id = instrument1Id,
                    ItemType = ItemType.Instrument,
                    IsAvailable = true,
                    Reviews = new List<Review>
                    {
                        new Review { Id = Guid.NewGuid(), Rating = 5, CreatedAt = DateTime.UtcNow, ItemId = instrument1Id, UserId = Guid.NewGuid() },
                        new Review { Id = Guid.NewGuid(), Rating = 4, CreatedAt = DateTime.UtcNow, ItemId = instrument1Id, UserId = Guid.NewGuid() }
                    }
                },
                new Item
                {
                    Id = instrument2Id,
                    ItemType = ItemType.Instrument,
                    IsAvailable = true,
                    Reviews = new List<Review>
                    {
                        new Review { Id = Guid.NewGuid(), Rating = 3, CreatedAt = DateTime.UtcNow, ItemId = instrument2Id, UserId = Guid.NewGuid() }
                    }
                },
                new Item
                {
                    Id = Guid.NewGuid(),
                    ItemType = ItemType.Album,
                    IsAvailable = true,
                    Reviews = new List<Review>
                    {
                        new Review { Id = Guid.NewGuid(), Rating = 5, CreatedAt = DateTime.UtcNow, ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() },
                        new Review { Id = Guid.NewGuid(), Rating = 4, CreatedAt = DateTime.UtcNow, ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() },
                        new Review { Id = Guid.NewGuid(), Rating = 3, CreatedAt = DateTime.UtcNow, ItemId = Guid.NewGuid(), UserId = Guid.NewGuid() }
                    }
                }
            }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(instruments.Object);

            // Act
            var result = await service.GetFeaturedInstrumentsAsync(2);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(i => i.ItemType == ItemType.Instrument), Is.True);

            // Check ordering by review count ascending
            Assert.That(result.ElementAt(0).Reviews.Count, Is.LessThanOrEqualTo(result.ElementAt(1).Reviews.Count));
        }

        [Test]
        public async Task CreateAsync_ShouldAddNewInstrumentWithImages()
        {
            // Arrange
            var dto = new CreateEditItemDto
            {
                Name = "Acoustic Guitar",
                Description = "Nice guitar",
                Price = 199.99m,
                BrandId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                ImageUrls = new List<string> { "url1", "url2" }
            };

            // Act
            await service.CreateAsync(dto);

            // Assert
            mockItemRepo.Verify(r => r.AddAsync(It.Is<Item>(i =>
                i.Name == dto.Name &&
                i.Description == dto.Description &&
                i.Price == dto.Price &&
                i.BrandId == dto.BrandId &&
                i.CategoryId == dto.CategoryId &&
                i.ItemType == ItemType.Instrument &&
                i.Images.Count == 2 &&
                i.Images.First().IsMain == true
            )), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyExistingInstrument()
        {
            // Arrange
            var id = Guid.NewGuid();
            var instrument = new Item
            {
                Id = id,
                Name = "Old Name",
                Description = "Old Desc",
                Price = 100,
                BrandId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                ItemType = ItemType.Instrument
            };

            var items = new List<Item> { instrument }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllTracked()).Returns(items.Object);

            var dto = new CreateEditItemDto
            {
                Id = id,
                Name = "New Name",
                Description = "New Desc",
                Price = 150,
                BrandId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                ItemType = ItemType.Instrument
            };

            // Act
            await service.UpdateAsync(dto);

            // Assert
            Assert.That(instrument.Name, Is.EqualTo(dto.Name));
            Assert.That(instrument.Description, Is.EqualTo(dto.Description));
            Assert.That(instrument.Price, Is.EqualTo(dto.Price));
            Assert.That(instrument.BrandId, Is.EqualTo(dto.BrandId));
            Assert.That(instrument.CategoryId, Is.EqualTo(dto.CategoryId));
            Assert.That(instrument.ItemType, Is.EqualTo(dto.ItemType));
            mockItemRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateAsync_ShouldThrow_WhenInstrumentNotFound()
        {
            // Arrange
            var dto = new CreateEditItemDto { Id = Guid.NewGuid() };
            var emptyItems = new List<Item>().AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllTracked()).Returns(emptyItems.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(dto));
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteInstrument_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var instrument = new Item { Id = id };

            mockItemRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(instrument);

            // Act
            await service.DeleteAsync(id);

            // Assert
            mockItemRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Test]
        public void DeleteAsync_ShouldThrow_WhenInstrumentNotFound()
        {
            // Arrange
            mockItemRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Item?)null);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => service.DeleteAsync(Guid.NewGuid()));
        }
    }
}