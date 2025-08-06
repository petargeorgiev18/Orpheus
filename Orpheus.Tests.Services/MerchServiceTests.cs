using MockQueryable.Moq;
using Moq;
using Orpheus.Core.DTOs;
using Orpheus.Core.Implementations;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class MerchServiceTests
    {
        private Mock<IRepository<Item, Guid>> mockItemRepo = null!;
        private MerchService service = null!;

        [SetUp]
        public void Setup()
        {
            mockItemRepo = new Mock<IRepository<Item, Guid>>();
            service = new MerchService(mockItemRepo.Object);
        }

        [Test]
        public async Task GetAvailableMerchAsync_ShouldReturnOnlyAvailableMerch()
        {
            // Arrange
            var merchItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Merch, IsAvailable = true },
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Merch, IsAvailable = false },
                new Item { Id = Guid.NewGuid(), ItemType = ItemType.Album, IsAvailable = true }
            }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(merchItems.Object);

            // Act
            var result = await service.GetAvailableMerchAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().ItemType, Is.EqualTo(ItemType.Merch));
            Assert.That(result.First().IsAvailable, Is.True);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectMerch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var merch = new Item { Id = id, Name = "Band T-Shirt", ItemType = ItemType.Merch };
            var items = new List<Item> { merch }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(items.Object);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Band T-Shirt", result!.Name);
        }

        [Test]
        public async Task CreateAsync_ShouldAddNewMerchWithImages()
        {
            // Arrange
            var dto = new CreateEditItemDto
            {
                Name = "Hoodie",
                Description = "Warm hoodie",
                Price = 49.99m,
                BrandId = Guid.NewGuid(),
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
                i.ItemType == ItemType.Merch &&
                i.Images.Count == 2 &&
                i.Images.First().IsMain == true
            )), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldModifyExistingMerch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var merch = new Item
            {
                Id = id,
                Name = "Old Hoodie",
                Description = "Old Desc",
                Price = 40,
                BrandId = Guid.NewGuid(),
                ItemType = ItemType.Merch
            };

            var items = new List<Item> { merch }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllTracked()).Returns(items.Object);

            var dto = new CreateEditItemDto
            {
                Id = id,
                Name = "New Hoodie",
                Description = "New Desc",
                Price = 55,
                BrandId = Guid.NewGuid(),
                ItemType = ItemType.Merch
            };

            // Act
            await service.UpdateAsync(dto);

            // Assert
            Assert.That(merch.Name, Is.EqualTo(dto.Name));
            Assert.That(merch.Description, Is.EqualTo(dto.Description));
            Assert.That(merch.Price, Is.EqualTo(dto.Price));
            Assert.That(merch.BrandId, Is.EqualTo(dto.BrandId));
            Assert.That(merch.ItemType, Is.EqualTo(dto.ItemType));
            mockItemRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void UpdateAsync_ShouldThrow_WhenMerchNotFound()
        {
            // Arrange
            var dto = new CreateEditItemDto { Id = Guid.NewGuid() };
            var emptyItems = new List<Item>().AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllTracked()).Returns(emptyItems.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(dto));
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteMerch_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var merch = new Item { Id = id };

            mockItemRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(merch);

            // Act
            await service.DeleteAsync(id);

            // Assert
            mockItemRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Test]
        public void DeleteAsync_ShouldThrow_WhenMerchNotFound()
        {
            // Arrange
            mockItemRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Item?)null);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => service.DeleteAsync(Guid.NewGuid()));
        }
    }
}
