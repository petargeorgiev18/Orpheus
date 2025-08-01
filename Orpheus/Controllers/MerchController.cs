using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models.Enums;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class MerchController : Controller
    {
        private readonly IMerchService merchService;

        public MerchController(IMerchService merchService)
        {
            this.merchService = merchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? sort, string? price, int page = 1, int pageSize = 6)
        {
            var merch = await merchService.GetAvailableMerchAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                merch = merch.Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                    || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(price))
            {
                merch = price.ToLower() switch
                {
                    "low" => merch.Where(i => i.Price < 20),
                    "mid" => merch.Where(i => i.Price >= 20 && i.Price <= 60),
                    "high" => merch.Where(i => i.Price > 60),
                    _ => merch
                };
            }

            merch = sort?.ToLower() switch
            {
                "priceasc" => merch.OrderBy(i => i.Price),
                "pricedesc" => merch.OrderByDescending(i => i.Price),
                _ => merch.OrderBy(i => i.Name)
            };

            int totalItems = merch.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (page < 1)
                page = 1;
            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var itemsOnPage = merch
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new ItemViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    BrandName = i.Brand?.Name ?? "Unknown",
                    ImageUrl = i.Images.FirstOrDefault()?.Url ?? "/images/default-image.png"
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm ?? "";
            ViewBag.Sort = sort ?? "";
            ViewBag.Price = price ?? "";
            return View(itemsOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await merchService.GetByIdAsync(id);

            if (item == null || item.ItemType != ItemType.Merch || !item.IsAvailable)
            {
                return NotFound();
            }

            var viewModel = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                BrandName = item.Brand?.Name ?? "Unknown",
                Images = item.Images.Select(i => i.Url).ToList()
            };

            return View(viewModel);
        }
    }
}