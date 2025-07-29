using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.ViewModels;
using System.Security.Claims;

namespace Orpheus.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IOrderService orderService;
        private readonly ICartService cartService;

        public CheckoutController(IOrderService orderService, ICartService cartService)
        {
            this.orderService = orderService;
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var cartItems = await cartService.GetCartItemsAsync(userId);

            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ItemId = ci.ItemId,
                    Name = ci.Name,
                    ImageUrl = ci.ImageUrl,
                    Price = ci.Price,
                    Quantity = ci.Quantity,
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return with errors
                return View("Index", model);
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            // Map ViewModel to CheckoutDto
            var checkoutDto = new Core.DTOs.CheckoutDto
            {
                UserId = userId,
                TotalAmount = model.GrandTotal,
                Items = model.CartItems.Select(ci => new Core.DTOs.CheckoutItemDto
                {
                    ItemId = ci.ItemId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList()
            };

            try
            {
                var orderId = await orderService.CreateOrderAsync(checkoutDto);

                // Optionally clear cart after order
                await cartService.ClearCartAsync(userId);

                return RedirectToAction("OrderSuccess", new { id = orderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", model);
            }
        }

        public IActionResult OrderSuccess(Guid id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}