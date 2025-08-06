using Moq;
using MockQueryable.Moq;
using Orpheus.Core.DTOs;
using Orpheus.Core.Implementations;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class AccessoryServiceTests
    {
        private Mock<IRepository<Item, Guid>> _mockItemRepository;
        private AccessoryService _service;

        [SetUp]
        public void Setup()
        {
            _mockItemRepository = new Mock<IRepository<Item, Guid>>();
            _service = new AccessoryService(_mockItemRepository.Object);
        }

        [Test]
        public async Task GetAvailableAccessoriesAsync_ShouldReturnOnlyAvailableAccessoriesWithIncludes()
        {
            // Arrange
            var items = new List<Item>
            {
                new Item
                {
                    Id = Guid.NewGuid(),
                    ItemType = ItemType.Accessory,
                    IsAvailable = true,
                    Category = new Category { CategoryName = "Strings" },
                    Images = new List<ItemImage> { new ItemImage { Url = "test.jpg" } },
                    Brand = new Brand { Name = "Fender" }
                },
                new Item
                {
                    Id = Guid.NewGuid(),
                    ItemType = ItemType.Accessory,
                    IsAvailable = false
                },
                new Item
                {
                    Id = Guid.NewGuid(),
                    ItemType = ItemType.Album,
                    IsAvailable = true
                }
            };

            var mockDbSet = items.AsQueryable().BuildMockDbSet();
            _mockItemRepository.Setup(r => r.GetAllAsNoTracking())
                .Returns(mockDbSet.Object);

            // Act
            var result = await _service.GetAvailableAccessoriesAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().ItemType, Is.EqualTo(ItemType.Accessory));
            Assert.That(result.First().IsAvailable, Is.True);
            Assert.That(result.First().Category, Is.Not.Null);
            Assert.That(result.First().Images, Is.Not.Empty);
            Assert.That(result.First().Brand, Is.Not.Null);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnItemWithIncludes_WhenItemExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new Item
            {
                Id = id,
                Name = "Guitar Strap",
                ItemType = ItemType.Accessory,
                Category = new Category { CategoryName = "Straps" },
                Images = new List<ItemImage> { new ItemImage { Url = "strap.jpg" } },
                Brand = new Brand { Name = "Gibson" }
            };

            var mockDbSet = new List<Item> { item }.AsQueryable().BuildMockDbSet();
            _mockItemRepository.Setup(r => r.GetAllAsNoTracking())
                .Returns(mockDbSet.Object);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Guitar Strap"));
            Assert.That(result.Category, Is.Not.Null);
            Assert.That(result.Images, Is.Not.Empty);
            Assert.That(result.Brand, Is.Not.Null);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenItemDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockDbSet = new List<Item>().AsQueryable().BuildMockDbSet();
            _mockItemRepository.Setup(r => r.GetAllAsNoTracking())
                .Returns(mockDbSet.Object);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_ShouldAddNewAccessoryWithImages()
        {
            // Arrange
            var dto = new CreateEditItemDto
            {
                Name = "Capo",
                Description = "A capo for guitar",
                Price = 15.99m,
                BrandId = Guid.NewGuid(),
                ImageUrls = new List<string> { "url1", "url2" }
            };

            _mockItemRepository.Setup(r => r.AddAsync(It.IsAny<Item>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _service.CreateAsync(dto);

            // Assert
            _mockItemRepository.Verify(r => r.AddAsync(It.Is<Item>(i =>
                i.Name == dto.Name &&
                i.Description == dto.Description &&
                i.Price == dto.Price &&
                i.BrandId == dto.BrandId &&
                i.ItemType == ItemType.Accessory &&
                i.IsAvailable &&
                i.Images.Count == 2 &&
                i.Images.First().IsMain == true &&
                i.Images.First().Url == "url1" &&
                i.Images.Last().Url == "url2"
            )), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ShouldAddNewAccessory_WhenNoImagesProvided()
        {
            // Arrange
            var dto = new CreateEditItemDto
            {
                Name = "Capo",
                Description = "A capo for guitar",
                Price = 15.99m,
                BrandId = Guid.NewGuid(),
                ImageUrls = null
            };

            _mockItemRepository.Setup(r => r.AddAsync(It.IsAny<Item>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _service.CreateAsync(dto);

            // Assert
            _mockItemRepository.Verify(r => r.AddAsync(It.Is<Item>(i =>
                i.Images.Count == 0
            )), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateExistingAccessory()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new Item
            {
                Id = id,
                Name = "Old Name",
                Description = "Old Desc",
                Price = 10,
                BrandId = Guid.NewGuid(),
                ItemType = ItemType.Accessory
            };

            var items = new List<Item> { item }.AsQueryable().BuildMockDbSet();
            _mockItemRepository.Setup(r => r.GetAllTracked())
                .Returns(items.Object);

            var dto = new CreateEditItemDto
            {
                Id = id,
                Name = "Updated Name",
                Description = "Updated Desc",
                Price = 20,
                BrandId = Guid.NewGuid()
            };

            // Act
            await _service.UpdateAsync(dto);

            // Assert
            Assert.That(item.Name, Is.EqualTo(dto.Name));
            Assert.That(item.Description, Is.EqualTo(dto.Description));
            Assert.That(item.Price, Is.EqualTo(dto.Price));
            Assert.That(item.BrandId, Is.EqualTo(dto.BrandId));
            Assert.That(item.ItemType, Is.EqualTo(ItemType.Accessory));
            _mockItemRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateAsync_ShouldThrowException_WhenAccessoryNotFound()
        {
            // Arrange
            var dto = new CreateEditItemDto { Id = Guid.NewGuid() };
            var items = new List<Item>().AsQueryable().BuildMockDbSet();
            _mockItemRepository.Setup(r => r.GetAllTracked())
                .Returns(items.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(dto));
            Assert.That(ex.Message, Is.EqualTo("Accessory not found"));
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteAccessory_WhenItemExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new Item { Id = id, Name = "To Delete" };

            _mockItemRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(item);
            _mockItemRepository.Setup(r => r.DeleteAsync(id))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _service.DeleteAsync(id);

            // Assert
            _mockItemRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Test]
        public void DeleteAsync_ShouldThrowException_WhenAccessoryNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockItemRepository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Item)null!);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(id));
            Assert.That(ex.Message, Is.EqualTo("Accessory not found"));
        }
    }
}