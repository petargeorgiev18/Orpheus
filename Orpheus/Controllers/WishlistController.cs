using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Implementations;
using Orpheus.Core.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService wishlistService;
        private readonly ICartService cartService;

        public WishlistController(IWishlistService wishlistService, ICartService cartService)
        {
            this.wishlistService = wishlistService;
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlistItems = await wishlistService.GetWishlistItemsAsync(userId);

            var model = wishlistItems.Select(i => new InstrumentViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                BrandName = i.Brand.Name,
                Images = i.Images.Select(img => img.Url).ToList(),
                ImageUrl = i.Images.FirstOrDefault()?.Url ?? "/images/default-image.png"
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishlist(Guid id)
        {
            string? userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;
            await wishlistService.AddToWishlistAsync(userId, id);
            return RedirectToAction("All", "Instruments");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishlist(Guid id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            await wishlistService.RemoveFromWishlistAsync(userId, id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMultipleToCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var userGuid = Guid.Parse(userId);

            var wishlistItems = await wishlistService.GetWishlistItemsAsync(userId);

            foreach (var item in wishlistItems)
            {
                await cartService.AddToCartAsync(item.Id, userGuid);
            }

            return RedirectToAction("Index");
        }

    }
}
