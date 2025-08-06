using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using MockQueryable.Moq;
using NUnit.Framework;
using Orpheus.Core.DTOs;
using Orpheus.Core.Implementations;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IRepository<Order, Guid>> _mockOrderRepo;
        private Mock<IRepository<OrderItem, Guid>> _mockOrderItemRepo;
        private Mock<IRepository<Item, Guid>> _mockItemRepo;
        private OrderService _service;

        [SetUp]
        public void Setup()
        {
            _mockOrderRepo = new Mock<IRepository<Order, Guid>>();
            _mockOrderItemRepo = new Mock<IRepository<OrderItem, Guid>>();
            _mockItemRepo = new Mock<IRepository<Item, Guid>>();

            _service = new OrderService(
                _mockOrderRepo.Object,
                _mockOrderItemRepo.Object,
                _mockItemRepo.Object);
        }

        [Test]
        public async Task CreateOrderAsync_ShouldCreateOrderAndReturnId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var items = new List<CheckoutItemDto>
    {
        new CheckoutItemDto { ItemId = Guid.NewGuid(), Quantity = 2, Price = 10m },
        new CheckoutItemDto { ItemId = Guid.NewGuid(), Quantity = 1, Price = 20m }
    };

            var checkoutDto = new CheckoutDto
            {
                UserId = userId,
                TotalAmount = 40m,
                FullName = "John Doe",
                Address = "123 Main St",
                City = "Cityville",
                PostalCode = "12345",
                PhoneNumber = "1234567890",
                PaymentMethod = "CreditCard",
                CardNumber = "1234567812345678",
                Items = items
            };

            Order capturedOrder = null!;

            _mockOrderRepo.Setup(r => r.AddWithoutSavingAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .Returns(Task.CompletedTask);

            _mockOrderRepo.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var resultId = await _service.CreateOrderAsync(checkoutDto);

            // Assert
            Assert.That(resultId, Is.EqualTo(capturedOrder.Id));
            Assert.That(capturedOrder.UserId, Is.EqualTo(userId));
            Assert.That(capturedOrder.Amount, Is.EqualTo(checkoutDto.TotalAmount));
            Assert.That(capturedOrder.FullName, Is.EqualTo(checkoutDto.FullName));
            Assert.That(capturedOrder.OrderStatus, Is.EqualTo(OrdersStatus.Pending));
            Assert.That(capturedOrder.PaymentStatus, Is.EqualTo(PaymentStatus.Paid));
            Assert.That(capturedOrder.CardNumber, Is.EqualTo("5678")); // Last 4 digits

            // Correct way to assert collection items
            var firstOrderItem = capturedOrder.OrderItems.First();
            var secondOrderItem = capturedOrder.OrderItems.Last();

            Assert.That(firstOrderItem.ItemId, Is.EqualTo(items[0].ItemId));
            Assert.That(firstOrderItem.Quantity, Is.EqualTo(items[0].Quantity));
            Assert.That(firstOrderItem.Price, Is.EqualTo(items[0].Price));

            Assert.That(secondOrderItem.ItemId, Is.EqualTo(items[1].ItemId));
            Assert.That(secondOrderItem.Quantity, Is.EqualTo(items[1].Quantity));
            Assert.That(secondOrderItem.Price, Is.EqualTo(items[1].Price));
        }

        [Test]
        public async Task CreateOrderAsync_ShouldNotStoreCardNumberForNonCreditCardPayments()
        {
            // Arrange
            var checkoutDto = new CheckoutDto
            {
                UserId = Guid.NewGuid(),
                TotalAmount = 50m,
                PaymentMethod = "Cash",
                CardNumber = "1234567890123456", // Should be ignored
                Items = new List<CheckoutItemDto>()
            };

            Order capturedOrder = null!;
            _mockOrderRepo.Setup(r => r.AddWithoutSavingAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateOrderAsync(checkoutDto);

            // Assert
            Assert.That(capturedOrder.CardNumber, Is.Null);
        }

        [Test]
        public async Task GetOrdersByUserAsync_ShouldReturnUserOrdersWithItems()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var item1 = new Item { Id = Guid.NewGuid(), Name = "Guitar" };
            var item2 = new Item { Id = Guid.NewGuid(), Name = "Strings" };

            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    OrderStatus = OrdersStatus.Delivered,
                    Amount = 100m,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { ItemId = item1.Id, Item = item1, Quantity = 1, Price = 100m }
                    }
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = OrdersStatus.Pending,
                    Amount = 20m,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem { ItemId = item2.Id, Item = item2, Quantity = 2, Price = 10m }
                    }
                }
            };

            var mockDbSet = orders.AsQueryable().BuildMockDbSet();
            _mockOrderRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(mockDbSet.Object);

            // Act
            var result = await _service.GetOrdersByUserAsync(userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].OrderDate, Is.GreaterThan(result[1].OrderDate)); // Descending order

            var firstOrder = result[0];
            Assert.That(firstOrder.Items.Count, Is.EqualTo(1));
            Assert.That(firstOrder.Items[0].Name, Is.EqualTo("Strings"));
            Assert.That(firstOrder.Items[0].Quantity, Is.EqualTo(2));
            Assert.That(firstOrder.Items[0].Price, Is.EqualTo(10m));
        }

        [Test]
        public async Task GetOrdersByUserAsync_ShouldReturnEmptyListForUserWithNoOrders()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var emptyOrders = new List<Order>().AsQueryable().BuildMockDbSet();

            _mockOrderRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(emptyOrders.Object);

            // Act
            var result = await _service.GetOrdersByUserAsync(userId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetOrdersByUserAsync_ShouldNotReturnOtherUsersOrders()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var orders = new List<Order>
            {
                new Order { UserId = userId },
                new Order { UserId = otherUserId }
            }.AsQueryable().BuildMockDbSet();

            _mockOrderRepo.Setup(r => r.GetAllAsNoTracking())
                .Returns(orders.Object);

            // Act
            var result = await _service.GetOrdersByUserAsync(userId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].OrderId, Is.EqualTo(orders.Object.First().Id));
        }
    }
}