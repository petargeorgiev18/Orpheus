using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Get current logged-in user's ID
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
    }
}
