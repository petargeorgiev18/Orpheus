using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Orpheus.Extensions;

namespace Orpheus.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            this.wishlistService = wishlistService;
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
    }
}
