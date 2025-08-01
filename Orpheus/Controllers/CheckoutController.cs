using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.DTOs;
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
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var cartItems = await cartService.GetCartItemsAsync(userId);

            if (!ModelState.IsValid)
            {
                model.CartItems = cartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ItemId = ci.ItemId,
                    Name = ci.Name,
                    ImageUrl = ci.ImageUrl,
                    Price = ci.Price,
                    Quantity = ci.Quantity
                }).ToList();

                return View("Index", model);
            }

            if (!cartItems.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return RedirectToAction("Index");
            }

            var totalAmount = cartItems.Sum(ci => ci.Price * ci.Quantity);

            var checkoutDto = new CheckoutDto
            {
                UserId = userId,
                TotalAmount = totalAmount,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                PhoneNumber = model.PhoneNumber,
                PaymentMethod = model.PaymentMethod,
                CardNumber = model.PaymentMethod == "CreditCard" ? model.CardNumber : null,
                ExpiryMonth = model.PaymentMethod == "CreditCard" ? model.ExpiryMonth : null,
                ExpiryYear = model.PaymentMethod == "CreditCard" ? model.ExpiryYear : null,
                CVV = model.PaymentMethod == "CreditCard" ? model.CVV : null,
                Items = cartItems.Select(ci => new Core.DTOs.CheckoutItemDto
                {
                    ItemId = ci.ItemId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList()
            };

            try
            {
                var orderId = await orderService.CreateOrderAsync(checkoutDto);
                await cartService.ClearCartAsync(userId);
                return RedirectToAction("OrderSuccess", new { id = orderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                model.CartItems = cartItems.Select(ci => new CartItemViewModel
                {
                    CartItemId = ci.CartItemId,
                    ItemId = ci.ItemId,
                    Name = ci.Name,
                    ImageUrl = ci.ImageUrl,
                    Price = ci.Price,
                    Quantity = ci.Quantity
                }).ToList();

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