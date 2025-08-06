using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using MockQueryable.Moq;
using NUnit.Framework;
using Orpheus.Core.Implementations;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using MockQueryable;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class WishlistServiceTests
    {
        private Mock<IRepository<Wishlist, Guid>> mockWishlistRepo = null!;
        private Mock<IRepository<WishlistItem, int>> mockWishlistItemRepo = null!;
        private Mock<IRepository<Item, Guid>> mockItemRepo = null!;
        private WishlistService service = null!;

        [SetUp]
        public void Setup()
        {
            mockWishlistRepo = new Mock<IRepository<Wishlist, Guid>>();
            mockWishlistItemRepo = new Mock<IRepository<WishlistItem, int>>();
            mockItemRepo = new Mock<IRepository<Item, Guid>>();

            service = new WishlistService(mockWishlistRepo.Object, mockWishlistItemRepo.Object, mockItemRepo.Object);
        }

        [Test]
        public async Task AddToWishlistAsync_ShouldReturnFalse_IfUserIdInvalid()
        {
            var result = await service.AddToWishlistAsync("invalid-guid", Guid.NewGuid());
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddToWishlistAsync_ShouldCreateWishlistAndAddItem_IfWishlistDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            // Setup wishlist query to return empty initially
            var wishlists = new List<Wishlist>().AsQueryable().BuildMock();
            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking()).Returns(wishlists);

            Wishlist? addedWishlist = null;
            mockWishlistRepo.Setup(r => r.AddAsync(It.IsAny<Wishlist>()))
                .Callback<Wishlist>(w => addedWishlist = w)
                .Returns(Task.CompletedTask);

            // After adding wishlist, it should be included in queries
            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(() => addedWishlist != null
                    ? new List<Wishlist> { addedWishlist }.AsQueryable().BuildMock()
                    : wishlists);

            // Setup wishlist items to return empty initially
            var wishlistItems = new List<WishlistItem>().AsQueryable().BuildMock();
            mockWishlistItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(wishlistItems);

            WishlistItem? addedWishlistItem = null;
            mockWishlistItemRepo.Setup(r => r.AddAsync(It.IsAny<WishlistItem>()))
                .Callback<WishlistItem>(wi => addedWishlistItem = wi)
                .Returns(Task.CompletedTask);

            var result = await service.AddToWishlistAsync(userId.ToString(), itemId);

            Assert.IsTrue(result);
            Assert.IsNotNull(addedWishlist);
            Assert.AreEqual(userId, addedWishlist!.UserId);
            Assert.IsNotNull(addedWishlistItem);
            Assert.AreEqual(addedWishlist.Id, addedWishlistItem!.WishlistId);
            Assert.AreEqual(itemId, addedWishlistItem.ItemId);
        }

        [Test]
        public async Task AddToWishlistAsync_ShouldReturnFalse_IfItemAlreadyInWishlist()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var wishlist = new Wishlist { Id = Guid.NewGuid(), UserId = userId };

            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist> { wishlist }.AsQueryable().BuildMock());

            mockWishlistItemRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<WishlistItem>
                {
                    new WishlistItem { WishlistId = wishlist.Id, ItemId = itemId }
                }.AsQueryable().BuildMock());

            var result = await service.AddToWishlistAsync(userId.ToString(), itemId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveFromWishlistAsync_ShouldReturnFalse_IfUserIdIsNullOrEmpty()
        {
            Assert.IsFalse(await service.RemoveFromWishlistAsync(null!, Guid.NewGuid()));
            Assert.IsFalse(await service.RemoveFromWishlistAsync(string.Empty, Guid.NewGuid()));
        }

        [Test]
        public async Task RemoveFromWishlistAsync_ShouldReturnFalse_IfWishlistNotFound()
        {
            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist>().AsQueryable().BuildMock());

            var result = await service.RemoveFromWishlistAsync(Guid.NewGuid().ToString(), Guid.NewGuid());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveFromWishlistAsync_ShouldReturnFalse_IfWishlistItemNotFound()
        {
            var userId = Guid.NewGuid();
            var wishlist = new Wishlist { Id = Guid.NewGuid(), UserId = userId };

            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist> { wishlist }.AsQueryable().BuildMock());

            mockWishlistItemRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<WishlistItem>().AsQueryable().BuildMock());

            var result = await service.RemoveFromWishlistAsync(userId.ToString(), Guid.NewGuid());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveFromWishlistAsync_ShouldDeleteWishlistItem_AndReturnTrue()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var wishlist = new Wishlist { Id = Guid.NewGuid(), UserId = userId };
            var wishlistItem = new WishlistItem { Id = 1, WishlistId = wishlist.Id, ItemId = itemId };

            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist> { wishlist }.AsQueryable().BuildMock());

            mockWishlistItemRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<WishlistItem> { wishlistItem }.AsQueryable().BuildMock());

            var deletedCalled = false;
            mockWishlistItemRepo.Setup(r => r.DeleteAsync(wishlistItem.Id))
                .Callback(() => deletedCalled = true)
                .Returns(Task.CompletedTask);

            var result = await service.RemoveFromWishlistAsync(userId.ToString(), itemId);

            Assert.IsTrue(result);
            Assert.IsTrue(deletedCalled);
        }

        [Test]
        public async Task IsInWishlistAsync_ShouldReturnFalse_IfUserIdNullOrEmpty()
        {
            Assert.IsFalse(await service.IsInWishlistAsync(null!, Guid.NewGuid()));
            Assert.IsFalse(await service.IsInWishlistAsync(string.Empty, Guid.NewGuid()));
        }

        [Test]
        public async Task IsInWishlistAsync_ShouldReturnFalse_IfWishlistNotFound()
        {
            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist>().AsQueryable().BuildMock());

            var result = await service.IsInWishlistAsync(Guid.NewGuid().ToString(), Guid.NewGuid());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsInWishlistAsync_ShouldReturnTrue_IfItemIsInWishlist()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var wishlist = new Wishlist { Id = Guid.NewGuid(), UserId = userId };

            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist> { wishlist }.AsQueryable().BuildMock());

            mockWishlistItemRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<WishlistItem>
                {
                    new WishlistItem { WishlistId = wishlist.Id, ItemId = itemId }
                }.AsQueryable().BuildMock());

            var result = await service.IsInWishlistAsync(userId.ToString(), itemId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetWishlistItemsAsync_ShouldReturnEmpty_IfUserIdNullOrEmpty()
        {
            var resultNull = await service.GetWishlistItemsAsync(null!);
            var resultEmpty = await service.GetWishlistItemsAsync(string.Empty);

            Assert.IsEmpty(resultNull);
            Assert.IsEmpty(resultEmpty);
        }

        [Test]
        public async Task GetWishlistItemsAsync_ShouldReturnEmpty_IfWishlistNotFound()
        {
            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist>().AsQueryable().BuildMock());

            var result = await service.GetWishlistItemsAsync(Guid.NewGuid().ToString());

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetWishlistItemsAsync_ShouldReturnItems_WhenWishlistFound()
        {
            var userId = Guid.NewGuid();
            var wishlist = new Wishlist { Id = Guid.NewGuid(), UserId = userId };
            var itemId1 = Guid.NewGuid();
            var itemId2 = Guid.NewGuid();

            mockWishlistRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(new List<Wishlist> { wishlist }.AsQueryable().BuildMock());

            var items = new List<Item>
            {
                new Item
                {
                    Id = itemId1,
                    Brand = new Brand { Name = "Brand1" },
                    Images = new List<ItemImage> { new ItemImage { Url = "url1" } },
                    WishlistsItems = new List<WishlistItem> { new WishlistItem { WishlistId = wishlist.Id, ItemId = itemId1 } }
                },
                new Item
                {
                    Id = itemId2,
                    Brand = new Brand { Name = "Brand2" },
                    Images = new List<ItemImage> { new ItemImage { Url = "url2" } },
                    WishlistsItems = new List<WishlistItem> { new WishlistItem { WishlistId = wishlist.Id, ItemId = itemId2 } }
                }
            };

            mockItemRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(items.AsQueryable().BuildMock());

            var result = (await service.GetWishlistItemsAsync(userId.ToString())).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(i => i.Id == itemId1));
            Assert.IsTrue(result.Any(i => i.Id == itemId2));
            Assert.IsTrue(result.All(i => i.WishlistsItems.Any(wi => wi.WishlistId == wishlist.Id)));
        }
    }
}