using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class CartService : ICartService
    {
        private readonly IRepository<CartItem, Guid> cartRepo;
        private readonly IRepository<Cart, Guid> cartRepository;
        public CartService(IRepository<CartItem, Guid> cartRepo, IRepository<Cart, Guid> cartRepository)
        {
            this.cartRepo = cartRepo;
            this.cartRepository = cartRepository;
        }
        public Task AddToCartAsync(Guid itemId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task ClearCartAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CartItemDto>> GetCartItemsAsync(Guid userId)
        {
            var cartItems = await cartRepo
                    .GetAllTracked()
                    .Where(c => c.Cart.UserId == userId)
                    .Include(c => c.Item)
                    .ThenInclude(i => i.Images)
                    .ToListAsync();

            return cartItems
                .Select(ci => new CartItemDto
                {
                    CartItemId = ci.Id,
                    ItemId = ci.ItemId,
                    Quantity = ci.Quantity,
                    Name = ci.Item.Name,
                    Price = ci.Item.Price,
                    ImageUrl = ci.Item.Images.FirstOrDefault()?.Url,
                })
                .ToList();
        }

        public Task RemoveFromCartAsync(Guid cartItemId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
