using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IItemService itemService;

        public CartController(ICartService cartService, IItemService itemService)
        {
            this.cartService = cartService;
            this.itemService = itemService;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var dtoItems = await cartService.GetCartItemsAsync(Guid.Parse(userId));

            var viewModel = dtoItems.Select(dto => new CartItemViewModel
            {
                CartItemId = dto.CartItemId,
                ItemId = dto.ItemId,
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Quantity = dto.Quantity
            }).ToList();

            return View(viewModel);
        }

        // POST: /Cart/Add
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            Console.WriteLine($"[DEBUG] AddToCart called with itemId: {id}");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            try
            {
                await cartService.AddToCartAsync(id, Guid.Parse(userId));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToAction("Index", "Albums");
        }

        // POST: /Cart/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(Guid cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await cartService.RemoveFromCartAsync(cartItemId, Guid.Parse(userId));

            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Clear
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await cartService.ClearCartAsync(Guid.Parse(userId));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(Guid cartItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            if (quantity < 1)
                quantity = 1; // prevent invalid values

            await cartService.UpdateQuantityAsync(cartItemId, quantity, Guid.Parse(userId));

            return RedirectToAction(nameof(Index));
        }

    }
}