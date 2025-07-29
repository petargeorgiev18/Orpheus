using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IItemService itemService;

        public CartController(ICartService cartService, IItemService itemService)
        {
            this.cartService = cartService;
            this.itemService = itemService;
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Remove(Guid cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await cartService.RemoveFromCartAsync(cartItemId, Guid.Parse(userId));

            return RedirectToAction(nameof(Index));
        }

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
                quantity = 1;

            await cartService.UpdateQuantityAsync(cartItemId, quantity, Guid.Parse(userId));

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var cartItemsDto = await cartService.GetCartItemsAsync(Guid.Parse(userId));

            if (!cartItemsDto.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            var cartItemsVm = cartItemsDto.Select(dto => new CartItemViewModel
            {
                CartItemId = dto.CartItemId,
                ItemId = dto.ItemId,
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Quantity = dto.Quantity
            }).ToList();

            var model = new CheckoutViewModel
            {
                CartItems = cartItemsVm
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var cartItemsDto = await cartService.GetCartItemsAsync(Guid.Parse(userId));
            if (!cartItemsDto.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.CartItems = cartItemsDto.Select(dto => new CartItemViewModel
                {
                    CartItemId = dto.CartItemId,
                    ItemId = dto.ItemId,
                    Name = dto.Name,
                    ImageUrl = dto.ImageUrl,
                    Price = dto.Price,
                    Quantity = dto.Quantity
                }).ToList();
                return View(model);
            }

            // TODO: Here you would process payment and create an order in DB

            // For now, simulate order success:
            await cartService.ClearCartAsync(Guid.Parse(userId));

            TempData["Success"] = "Thank you for your purchase! Your order has been placed.";

            return RedirectToAction("Index", "Home");
        }
    }
}