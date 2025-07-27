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
        public async Task<IActionResult> Index(string? searchTerm, string? sort)
        {
            var accessories = await accessoryService.GetAvailableAccessoriesAsync();

            // Filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                accessories = accessories
                    .Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Sort
            accessories = sort?.ToLower() switch
            {
                "priceasc" => accessories.OrderBy(i => i.Price),
                "pricedesc" => accessories.OrderByDescending(i => i.Price),
                _ => accessories.OrderBy(i => i.Name)
            };

            // Project to ViewModel
            var viewModel = accessories.Select(i => new InstrumentViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                BrandName = i.Brand?.Name ?? "Unknown",
                ImageUrl = i.Images
                            .OrderByDescending(img => img.IsMain) // Main image first
                            .Select(img => img.Url)
                            .FirstOrDefault() ?? "/images/default-image.png"
            });

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await accessoryService.GetByIdAsync(id);

            if (item == null || item.ItemType != ItemType.Accessory || !item.IsAvailable)
            {
                return NotFound();
            }

            var viewModel = new InstrumentViewModel
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