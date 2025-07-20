using Orpheus.Core.DTOs;

namespace Orpheus.Core.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartItemsAsync(Guid userId);

        Task AddToCartAsync(Guid itemId, string userId);

        Task RemoveFromCartAsync(Guid cartItemId, string userId);

        Task ClearCartAsync(string userId);
    }
}
