using Orpheus.Core.DTOs;

namespace Orpheus.Core.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartItemsAsync(Guid userId);
        Task AddToCartAsync(Guid itemId, Guid userId);
        Task RemoveFromCartAsync(Guid cartItemId, Guid userId);
        Task ClearCartAsync(Guid userId);
        Task UpdateQuantityAsync(Guid cartItemId, int quantity, Guid userId);
    }
}