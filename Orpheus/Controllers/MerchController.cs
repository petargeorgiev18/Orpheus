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
        public async Task<IActionResult> Index(string? searchTerm, string? sort)
        {
            var merch = await merchService.GetAvailableMerchAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                merch = merch
                    .Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                merch = sort.ToLower() switch
                {
                    "priceasc" => merch.OrderBy(i => i.Price),
                    "pricedesc" => merch.OrderByDescending(i => i.Price),
                    _ => merch
                };
            }

            var viewModel = merch.Select(i => new InstrumentViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                BrandName = i.Brand?.Name ?? "Unknown",
                ImageUrl = i.Images.FirstOrDefault()?.Url ?? "/images/default-image.png"
            });

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await merchService.GetByIdAsync(id);

            if (item == null || item.ItemType != ItemType.Merch || !item.IsAvailable)
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
                Images = item.Images.Select(i => i.Url).ToList()
            };

            return View(viewModel);
        }
    }
}