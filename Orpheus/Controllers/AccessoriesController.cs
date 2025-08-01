using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models.Enums;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly IAccessoryService accessoryService;

        public AccessoriesController(IAccessoryService accessoryService)
        {
            this.accessoryService = accessoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? sort, string? price, int page = 1, int pageSize = 6)
        {
            var accessories = await accessoryService.GetAvailableAccessoriesAsync();

            // Filtering: search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                accessories = accessories
                    .Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Filtering: price
            if (!string.IsNullOrEmpty(price))
            {
                accessories = price.ToLower() switch
                {
                    "low" => accessories.Where(i => i.Price < 20),
                    "mid" => accessories.Where(i => i.Price >= 20 && i.Price <= 60),
                    "high" => accessories.Where(i => i.Price > 60),
                    _ => accessories
                };
            }

            accessories = sort?.ToLower() switch
            {
                "priceasc" => accessories.OrderBy(i => i.Price),
                "pricedesc" => accessories.OrderByDescending(i => i.Price),
                _ => accessories.OrderBy(i => i.Name) // Default sort by name
            };

            int totalItems = accessories.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            page = Math.Clamp(page, 1, totalPages > 0 ? totalPages : 1);

            var itemsOnPage = accessories
                .OrderBy(i=>i.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new ItemViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    BrandName = i.Brand?.Name ?? "Unknown",
                    ImageUrl = i.Images
                        .OrderByDescending(img => img.IsMain)
                        .Select(img => img.Url)
                        .FirstOrDefault() ?? "/images/default-image.png"
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
            var item = await accessoryService.GetByIdAsync(id);

            if (item == null || item.ItemType != ItemType.Accessory || !item.IsAvailable)
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
                Images = item.Images
                    .OrderByDescending(i => i.IsMain)
                    .Select(i => i.Url)
                    .ToList()
            };

            return View(viewModel);
        }
    }
}