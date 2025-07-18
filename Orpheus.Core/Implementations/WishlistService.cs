using Microsoft.EntityFrameworkCore;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orpheus.Core.Implementations
{
    public class WishlistService : IWishlistService
    {
        private readonly IRepository<Wishlist, Guid> wishlistRepo;
        private readonly IRepository<WishlistItem, int> wishlistItemRepo;
        private readonly IRepository<Item, Guid> itemRepo;

        public WishlistService(
            IRepository<Wishlist, Guid> wishlistRepo,
            IRepository<WishlistItem, int> wishlistItemRepo,
            IRepository<Item, Guid> itemRepo)
        {
            this.wishlistRepo = wishlistRepo;
            this.wishlistItemRepo = wishlistItemRepo;
            this.itemRepo = itemRepo;
        }

        public async Task<bool> AddToWishlistAsync(string userId, Guid itemId)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return false;

            var wishlist = await wishlistRepo.GetAllAsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId == userGuid);

            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = userGuid };
                await wishlistRepo.AddAsync(wishlist);
            }

            bool exists = await wishlistItemRepo.GetAllAsNoTracking()
                .AnyAsync(wi => wi.WishlistId == wishlist.Id && wi.ItemId == itemId);

            if (exists)
                return false;

            var wishlistItem = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ItemId = itemId
            };

            await wishlistItemRepo.AddAsync(wishlistItem);

            return true;
        }


        public async Task<bool> RemoveFromWishlistAsync(string userId, Guid itemId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            var wishlist = await wishlistRepo.GetAllAsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId.ToString() == userId);

            if (wishlist == null)
                return false;

            var wishlistItem = await wishlistItemRepo.GetAllAsNoTracking()
                .FirstOrDefaultAsync(wi => wi.WishlistId == wishlist.Id && wi.ItemId == itemId);

            if (wishlistItem == null)
                return false;

            await wishlistItemRepo.DeleteAsync(wishlistItem.Id);

            return true;
        }

        public async Task<bool> IsInWishlistAsync(string userId, Guid itemId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            var wishlist = await wishlistRepo.GetAllAsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId.ToString() == userId);

            if (wishlist == null)
                return false;

            return await wishlistItemRepo.GetAllAsNoTracking()
                .AnyAsync(wi => wi.WishlistId == wishlist.Id && wi.ItemId == itemId);
        }

        public async Task<IEnumerable<Item>> GetWishlistItemsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<Item>();

            var wishlist = await wishlistRepo.GetAllAsNoTracking()
                .FirstOrDefaultAsync(w => w.UserId.ToString() == userId);

            if (wishlist == null)
                return Enumerable.Empty<Item>();

            var items = await itemRepo.GetAllAsNoTracking()
                .Where(i => i.WishlistsItems.Any(wi => wi.WishlistId == wishlist.Id))
                .Include(i => i.Brand)
                .Include(i => i.Images)
                .ToListAsync();

            return items;
        }
    }
}