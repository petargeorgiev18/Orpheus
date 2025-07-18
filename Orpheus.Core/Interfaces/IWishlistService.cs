using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orpheus.Data.Models;

namespace Orpheus.Core.Interfaces
{
    public interface IWishlistService
    {
        Task<bool> AddToWishlistAsync(string? userId, Guid itemId);
        Task<bool> RemoveFromWishlistAsync(string? userId, Guid itemId);
        Task<bool> IsInWishlistAsync(string? userId, Guid itemId);
        Task<IEnumerable<Item>> GetWishlistItemsAsync(string? userId);
    }
}
