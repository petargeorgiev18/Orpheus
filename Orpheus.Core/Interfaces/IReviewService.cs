using Orpheus.Core.DTOs;

namespace Orpheus.Core.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(ReviewDto dto);
        Task<IEnumerable<ReviewDto>> GetReviewsByItemIdAsync(Guid itemId);
    }
}