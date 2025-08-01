using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models.Enums;
using Orpheus.ViewModels;
using System.Security.Claims;

namespace Orpheus.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly IItemService itemService;
        public ReviewsController(IReviewService reviewService, IItemService itemService)
        {
            this.reviewService = reviewService;
            this.itemService = itemService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ReviewFormViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var item = await itemService.GetByIdAsync(model.ItemId);
            if (item == null || !item.IsAvailable)
                return NotFound();

            string controller = item.ItemType switch
            {
                ItemType.Instrument => "Instruments",
                ItemType.Album => "Albums",
                ItemType.Accessory => "Accessories",
                ItemType.Merch => "Merch",
                _ => "Home"
            };

            if (!ModelState.IsValid)
            {
                TempData["ReviewError"] = "Please provide a valid rating (1–5) and/or a comment.";
                return RedirectToAction("Details", controller, new { id = model.ItemId });
            }

            var userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                await reviewService.AddReviewAsync(new ReviewDto
                {
                    ItemId = model.ItemId,
                    UserId = userId,
                    Rating = model.Rating,
                    Comment = model.Comment
                });
            }
            catch (InvalidOperationException ex)
            {
                TempData["ReviewError"] = ex.Message;
            }

            return RedirectToAction("Details", controller, new { id = model.ItemId });
        }
    }
}