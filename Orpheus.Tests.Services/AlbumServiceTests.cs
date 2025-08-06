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
    public class AlbumServiceTests
    {
        private Mock<IRepository<Item, Guid>> mockRepo;
        private AlbumService service;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IRepository<Item, Guid>>();
            service = new AlbumService(mockRepo.Object);
        }

        [Test]
        public async Task GetAvailableAlbumsAsync_ShouldReturnOnlyAvailableAlbums()
        {
            // Arrange
            var albums = new List<Item>
            {
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Album, IsAvailable = true },
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Album, IsAvailable = false },
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Accessory, IsAvailable = true }
            }.AsQueryable();

            var mockDbSet = albums.BuildMockDbSet();
            mockRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet.Object);

            // Act
            var result = await service.GetAvailableAlbumsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().ItemType, Is.EqualTo(ItemType.Album));
            Assert.That(result.First().IsAvailable, Is.True);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectAlbum()
        {
            // Arrange
            var id = Guid.NewGuid();
            var album = new Item { Id = id, Name = "Test Album", ItemType = ItemType.Album };
            var albums = new List<Item> { album }.AsQueryable().BuildMockDbSet();

            mockRepo.Setup(r => r.GetAllAsNoTracking()).Returns(albums.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Album"));
        }

        [Test]
        public async Task CreateAsync_ShouldAddNewAlbum()
        {
            // Arrange
            var dto = new CreateEditItemDto
            {
                Name = "New Album",
                Description = "Great album",
                Price = 19.99m,
                BrandId = Guid.NewGuid(),
                ImageUrls = new List<string> { "url1", "url2" }
            };

            // Act
            await service.CreateAsync(dto);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.Is<Item>(i =>
                i.Name == dto.Name &&
                i.Description == dto.Description &&
                i.Price == dto.Price &&
                i.BrandId == dto.BrandId &&
                i.ItemType == ItemType.Album &&
                i.Images.Count == 2 &&
                (i.Images.First().IsMain ?? false)
            )), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingAlbum()
        {
            // Arrange
            var id = Guid.NewGuid();
            var album = new Item
            {
                Id = id,
                Name = "Old Name",
                Description = "Old Description",
                Price = 10,
                BrandId = Guid.NewGuid(),
                ItemType = ItemType.Album
            };

            var albums = new List<Item> { album }.AsQueryable().BuildMockDbSet();
            mockRepo.Setup(r => r.GetAllTracked()).Returns(albums.Object);

            var dto = new CreateEditItemDto
            {
                Id = id,
                Name = "Updated Name",
                Description = "Updated Description",
                Price = 25,
                BrandId = Guid.NewGuid(),
                ItemType = ItemType.Album
            };

            // Act
            await service.UpdateAsync(dto);

            // Assert
            Assert.That(album.Name, Is.EqualTo(dto.Name));
            Assert.That(album.Description, Is.EqualTo(dto.Description));
            Assert.That(album.Price, Is.EqualTo(dto.Price));
            Assert.That(album.BrandId, Is.EqualTo(dto.BrandId));
            Assert.That(album.ItemType, Is.EqualTo(dto.ItemType));

            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateAsync_ShouldThrowException_WhenAlbumNotFound()
        {
            // Arrange
            var dto = new CreateEditItemDto { Id = Guid.NewGuid() };
            var emptyAlbums = new List<Item>().AsQueryable().BuildMockDbSet();

            mockRepo.Setup(r => r.GetAllTracked()).Returns(emptyAlbums.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(dto));
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteAlbum()
        {
            // Arrange
            var id = Guid.NewGuid();
            var album = new Item { Id = id, Name = "To Delete" };

            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(album);

            // Act
            await service.DeleteAsync(id);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Test]
        public void DeleteAsync_ShouldThrowException_WhenAlbumNotFound()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null!);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => service.DeleteAsync(Guid.NewGuid()));
        }
    }
}