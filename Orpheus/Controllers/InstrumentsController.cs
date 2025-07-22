using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models.Enums;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class InstrumentsController : Controller
    {
        private readonly IInstrumentService instrumentItemService;

        public InstrumentsController(IInstrumentService instrumentItemService)
        {
            this.instrumentItemService = instrumentItemService;
        }
        [HttpGet]
        public async Task<IActionResult> All(string? type, string? brand, string? price)
        {
            var instruments = await instrumentItemService.GetAvailableInstrumentsAsync();

            if (!string.IsNullOrEmpty(type))
            {
                instruments = instruments
                    .Where(i => i.Category != null &&
                                i.Category.CategoryName.Contains(type, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(brand))
            {
                instruments = instruments
                    .Where(i => i.Brand != null &&
                                i.Brand.Name.Contains(brand, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(price))
            {
                instruments = price.ToLower() switch
                {
                    "low" => instruments.Where(i => i.Price < 200),
                    "mid" => instruments.Where(i => i.Price >= 200 && i.Price <= 1000),
                    "high" => instruments.Where(i => i.Price > 1000),
                    _ => instruments
                };
            }

            var viewModel = instruments.Select(i => new InstrumentViewModel
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
            var item = await instrumentItemService.GetByIdAsync(id);
            if (item == null || item.ItemType != ItemType.Instrument || !item.IsAvailable)
            {
                return NotFound();
            }
            var viewModel = new InstrumentViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                BrandName = item.Brand?.Name ?? "Unknown Brand",
                Images = item.Images != null && item.Images.Any()
                    ? item.Images.Select(img => img.Url).ToList()
                    : new List<string> { "/images/default-image.png" }
            };
            return View(viewModel);
        }
    }
}
