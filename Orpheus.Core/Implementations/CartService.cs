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
        private readonly IRepository<Item, Guid> itemRepository;
        public CartService(IRepository<CartItem, Guid> cartRepo, IRepository<Cart, Guid> cartRepository, IRepository<Item, Guid> itemRepository)
        {
            this.cartRepo = cartRepo;
            this.cartRepository = cartRepository;
            this.itemRepository = itemRepository;
        }
        public async Task AddToCartAsync(Guid itemId, Guid userId)
        {
            var item = await itemRepository
                .GetAllAsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == itemId && i.IsAvailable == true);

            if (item == null)
            {
                throw new InvalidOperationException("Item not found at all.");
            }

            var cart = await cartRepository
                .GetAllTracked()
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };
                await cartRepository.AddAsync(cart);
            }

            var existingItem = await cartRepo
                .GetAllTracked()
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ItemId == itemId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                await cartRepo.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ItemId = itemId,
                    Quantity = 1
                };
                await cartRepo.AddAsync(newItem);
            }
        }


        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await cartRepository
                            .GetAllTracked()
                            .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItems = await cartRepo
                    .GetAllTracked()
                    .Where(ci => ci.CartId == cart.Id)
                    .ToListAsync();

                foreach (var item in cartItems)
                {
                    await cartRepo.DeleteAsync(item.Id);
                }
            }
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

        public async Task RemoveFromCartAsync(Guid cartItemId, Guid userId)
        {
            var cartItem = await cartRepo
                            .GetAllTracked()
                            .Include(ci => ci.Cart)
                            .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);

            if (cartItem != null)
            {
                await cartRepo.DeleteAsync(cartItem.Id);
            }
        }

        public async Task UpdateQuantityAsync(Guid cartItemId, int quantity, Guid userId)
        {
            var cartItem = await cartRepo.GetAllTracked() // Use cartRepo here
                .Include(ci => ci.Cart) // Include Cart navigation to check ownership
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cartItem == null || cartItem.Cart.UserId != userId)
                throw new Exception("Cart item not found or access denied.");

            cartItem.Quantity = quantity;

            await cartRepo.UpdateAsync(cartItem); // Use cartRepo here to update
        }

    }
}
