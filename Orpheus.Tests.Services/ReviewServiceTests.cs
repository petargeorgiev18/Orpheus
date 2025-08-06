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
using Orpheus.Data.Repository.Interfaces;
using MockQueryable;

namespace Orpheus.Tests.Services
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private Mock<IRepository<Review, Guid>> _mockReviewRepo;
        private ReviewService _service;

        [SetUp]
        public void Setup()
        {
            _mockReviewRepo = new Mock<IRepository<Review, Guid>>();
            _service = new ReviewService(_mockReviewRepo.Object);
        }

        [Test]
        public async Task UserHasReviewedAsync_ShouldReturnTrue_IfReviewExists()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var reviews = new List<Review>
            {
                new Review { ItemId = itemId, UserId = userId, IsDeleted = false }
            };

            var mockDbSet = reviews.AsQueryable().BuildMock();
            _mockReviewRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet);

            // Act
            var result = await _service.UserHasReviewedAsync(itemId, userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UserHasReviewedAsync_ShouldReturnFalse_IfNoReview()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockDbSet = new List<Review>().AsQueryable().BuildMock();
            _mockReviewRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet);

            // Act
            var result = await _service.UserHasReviewedAsync(itemId, userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UserHasReviewedAsync_ShouldReturnFalse_IfReviewIsDeleted()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var reviews = new List<Review>
            {
                new Review { ItemId = itemId, UserId = userId, IsDeleted = true }
            };

            var mockDbSet = reviews.AsQueryable().BuildMock();
            _mockReviewRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet);

            // Act
            var result = await _service.UserHasReviewedAsync(itemId, userId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddReviewAsync_ShouldThrow_IfUserAlreadyReviewed()
        {
            // Arrange
            var dto = new ReviewDto
            {
                ItemId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Nice!"
            };

            var reviews = new List<Review>
            {
                new Review { ItemId = dto.ItemId, UserId = dto.UserId, IsDeleted = false }
            };

            var mockDbSet = reviews.AsQueryable().BuildMock();
            _mockReviewRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddReviewAsync(dto));
            Assert.That(ex.Message, Is.EqualTo("You have already reviewed this item."));
        }

        [Test]
        public async Task AddReviewAsync_ShouldAddReview_WhenNotReviewedBefore()
        {
            // Arrange
            var dto = new ReviewDto
            {
                ItemId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Great product!"
            };

            var mockDbSet = new List<Review>().AsQueryable().BuildMock();
            _mockReviewRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet);

            Review? addedReview = null;
            _mockReviewRepo.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Callback<Review>(r => addedReview = r)
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddReviewAsync(dto);

            // Assert
            Assert.IsNotNull(addedReview);
            Assert.AreEqual(dto.ItemId, addedReview!.ItemId);
            Assert.AreEqual(dto.UserId, addedReview.UserId);
            Assert.AreEqual(dto.Rating, addedReview.Rating);
            Assert.AreEqual(dto.Comment, addedReview.Comment);
            Assert.IsFalse(addedReview.IsDeleted);
            Assert.That((DateTime.UtcNow - addedReview.CreatedAt).TotalSeconds, Is.LessThan(5));
        }

        [Test]
        public async Task GetReviewsByItemIdAsync_ShouldReturnReviewsOrdered()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var user1 = new OrpheusAppUser { Id = Guid.NewGuid(), UserName = "UserOne" };
            var user2 = new OrpheusAppUser { Id = Guid.NewGuid(), UserName = "UserTwo" };

            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    ItemId = itemId,
                    UserId = user1.Id,
                    User = user1,
                    Rating = 5,
                    Comment = "Excellent",
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                    IsDeleted = false
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    ItemId = itemId,
                    UserId = user2.Id,
                    User = user2,
                    Rating = 3,
                    Comment = "Good",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    ItemId = itemId,
                    UserId = user2.Id,
                    User = user2,
                    Rating = 1,
                    Comment = "Deleted review",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = true
                }
            };

            var mockDbSet = reviews.AsQueryable().BuildMock();
            _mockReviewRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet);

            // Act
            var result = await _service.GetReviewsByItemIdAsync(itemId);

            // Assert
            Assert.AreEqual(2, result.Count()); // Only 2 non-deleted reviews
            Assert.AreEqual("UserTwo", result.First().UserFullName); // Ordered by CreatedAt descending
            Assert.AreEqual("Good", result.First().Comment);
            Assert.AreEqual("UserOne", result.Last().UserFullName);
            Assert.AreEqual("Excellent", result.Last().Comment);
        }

        [Test]
        public async Task GetReviewsByItemIdAsync_ShouldReturnEmpty_WhenNoValidReviews()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var reviews = new List<Review>
            {
                new Review { ItemId = itemId, IsDeleted = true },
                new Review { ItemId = Guid.NewGuid(), IsDeleted = false } // Different item
            };

            var mockDbSet = reviews.AsQueryable().BuildMock();
            _mockReviewRepo.Setup(r => r.GetAllAsNoTracking()).Returns(mockDbSet);

            // Act
            var result = await _service.GetReviewsByItemIdAsync(itemId);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}