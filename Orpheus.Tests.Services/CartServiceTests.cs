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
using Orpheus.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<IRepository<CartItem, Guid>> mockCartItemRepo;
        private Mock<IRepository<Cart, Guid>> mockCartRepo;
        private Mock<IRepository<Item, Guid>> mockItemRepo;
        private CartService service;

        [SetUp]
        public void Setup()
        {
            mockCartItemRepo = new Mock<IRepository<CartItem, Guid>>();
            mockCartRepo = new Mock<IRepository<Cart, Guid>>();
            mockItemRepo = new Mock<IRepository<Item, Guid>>();
            service = new CartService(mockCartItemRepo.Object, mockCartRepo.Object, mockItemRepo.Object);
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddNewCartAndCartItem_WhenNoCartExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var itemList = new List<Item> {
                new Item { Id = itemId, IsAvailable = true }
            }.AsQueryable().BuildMockDbSet();

            var emptyCarts = new List<Cart>().AsQueryable().BuildMockDbSet();
            var emptyCartItems = new List<CartItem>().AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(itemList.Object);
            mockCartRepo.Setup(r => r.GetAllTracked()).Returns(emptyCarts.Object);
            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(emptyCartItems.Object);

            // Act
            await service.AddToCartAsync(itemId, userId);

            // Assert
            mockCartRepo.Verify(r => r.AddAsync(It.Is<Cart>(c => c.UserId == userId)), Times.Once);
            mockCartItemRepo.Verify(r => r.AddAsync(It.Is<CartItem>(ci => ci.ItemId == itemId)), Times.Once);
        }

        [Test]
        public async Task AddToCartAsync_ShouldIncrementQuantity_WhenCartItemExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            var itemList = new List<Item> {
                new Item { Id = itemId, IsAvailable = true }
            }.AsQueryable().BuildMockDbSet();

            var carts = new List<Cart> {
                new Cart { Id = cartId, UserId = userId }
            }.AsQueryable().BuildMockDbSet();

            var cartItems = new List<CartItem> {
                new CartItem { Id = Guid.NewGuid(), CartId = cartId, ItemId = itemId, Quantity = 1 }
            }.AsQueryable().BuildMockDbSet();

            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(itemList.Object);
            mockCartRepo.Setup(r => r.GetAllTracked()).Returns(carts.Object);
            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(cartItems.Object);

            // Act
            await service.AddToCartAsync(itemId, userId);

            // Assert
            mockCartItemRepo.Verify(r => r.UpdateAsync(It.Is<CartItem>(ci => ci.Quantity == 2)), Times.Once);
            mockCartItemRepo.Verify(r => r.AddAsync(It.IsAny<CartItem>()), Times.Never);
        }

        [Test]
        public void AddToCartAsync_ShouldThrow_WhenItemNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var emptyItems = new List<Item>().AsQueryable().BuildMockDbSet();
            mockItemRepo.Setup(r => r.GetAllAsNoTracking()).Returns(emptyItems.Object);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => service.AddToCartAsync(itemId, userId));
        }

        [Test]
        public async Task ClearCartAsync_ShouldDeleteAllCartItems()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            var carts = new List<Cart> {
                new Cart { Id = cartId, UserId = userId }
            }.AsQueryable().BuildMockDbSet();

            var cartItemsList = new List<CartItem> {
                new CartItem { Id = Guid.NewGuid(), CartId = cartId },
                new CartItem { Id = Guid.NewGuid(), CartId = cartId }
            }.AsQueryable().BuildMockDbSet();

            mockCartRepo.Setup(r => r.GetAllTracked()).Returns(carts.Object);
            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(cartItemsList.Object);

            // Act
            await service.ClearCartAsync(userId);

            // Assert
            foreach (var ci in cartItemsList.Object)
            {
                mockCartItemRepo.Verify(r => r.DeleteAsync(ci.Id), Times.Once);
            }
        }

        [Test]
        public async Task GetCartItemsAsync_ShouldReturnCorrectDtoList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            var items = new List<ItemImage> { new ItemImage { Url = "img.jpg" } };

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cartId,
                    ItemId = itemId,
                    Quantity = 3,
                    Cart = new Cart { Id = cartId, UserId = userId },
                    Item = new Item { Id = itemId, Name = "TestItem", Price = 100, Images = items }
                }
            }.AsQueryable().BuildMockDbSet();

            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(cartItems.Object);

            // Act
            var result = await service.GetCartItemsAsync(userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            var dto = result.First();
            Assert.That(dto.CartItemId, Is.EqualTo(cartItems.Object.First().Id));
            Assert.That(dto.ItemId, Is.EqualTo(itemId));
            Assert.That(dto.Quantity, Is.EqualTo(3));
            Assert.That(dto.Name, Is.EqualTo("TestItem"));
            Assert.That(dto.Price, Is.EqualTo(100));
            Assert.That(dto.ImageUrl, Is.EqualTo("img.jpg"));
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldDeleteItem_WhenExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = cartItemId,
                    Cart = new Cart { UserId = userId }
                }
            }.AsQueryable().BuildMockDbSet();

            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(cartItems.Object);

            // Act
            await service.RemoveFromCartAsync(cartItemId, userId);

            // Assert
            mockCartItemRepo.Verify(r => r.DeleteAsync(cartItemId), Times.Once);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldDoNothing_WhenItemNotExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            var emptyCartItems = new List<CartItem>().AsQueryable().BuildMockDbSet();

            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(emptyCartItems.Object);

            // Act
            await service.RemoveFromCartAsync(cartItemId, userId);

            // Assert
            mockCartItemRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public async Task UpdateQuantityAsync_ShouldUpdateQuantity_WhenCartItemFoundAndUserMatches()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = cartItemId,
                    Quantity = 1,
                    Cart = new Cart { UserId = userId }
                }
            }.AsQueryable().BuildMockDbSet();

            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(cartItems.Object);

            // Act
            await service.UpdateQuantityAsync(cartItemId, 5, userId);

            // Assert
            mockCartItemRepo.Verify(r => r.UpdateAsync(It.Is<CartItem>(ci => ci.Quantity == 5)), Times.Once);
        }

        [Test]
        public void UpdateQuantityAsync_ShouldThrow_WhenCartItemNotFoundOrUserMismatch()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = cartItemId,
                    Quantity = 1,
                    Cart = new Cart { UserId = Guid.NewGuid() } // Different user id
                }
            }.AsQueryable().BuildMockDbSet();

            mockCartItemRepo.Setup(r => r.GetAllTracked()).Returns(cartItems.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => service.UpdateQuantityAsync(cartItemId, 5, userId));
        }
    }
}