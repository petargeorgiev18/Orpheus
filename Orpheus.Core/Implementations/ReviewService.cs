using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review, Guid> reviewRepo;

        public ReviewService(IRepository<Review, Guid> reviewRepo)
        {
            this.reviewRepo = reviewRepo;
        }

        public async Task<bool> UserHasReviewedAsync(Guid itemId, Guid userId)
        {
            return await reviewRepo.GetAllAsNoTracking()
                .AnyAsync(r => r.ItemId == itemId && r.UserId == userId && !r.IsDeleted);
        }

        public async Task AddReviewAsync(ReviewDto dto)
        {
            if (await UserHasReviewedAsync(dto.ItemId, dto.UserId))
            {
                throw new InvalidOperationException("You have already reviewed this item.");
            }

            var review = new Review
            {
                Id = Guid.NewGuid(),
                ItemId = dto.ItemId,
                UserId = dto.UserId,
                Comment = dto.Comment,
                Rating = dto.Rating,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            await reviewRepo.AddAsync(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByItemIdAsync(Guid itemId)
        {
            return await reviewRepo.GetAllAsNoTracking()
                .Where(r => r.ItemId == itemId && !r.IsDeleted)
                .Include(r => r.User)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ItemId = r.ItemId,
                    UserId = r.UserId,
                    UserFullName = r.User.UserName!,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}