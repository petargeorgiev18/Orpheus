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
        public async Task<IActionResult> All(string? category, string? brand, string? price, int page = 1, int pageSize = 6)
        {
            var instruments = await instrumentItemService.GetAvailableInstrumentsAsync();

            if (!string.IsNullOrEmpty(category))
                instruments = instruments.Where(i => i.Category?.CategoryName.Contains(category, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrEmpty(brand))
                instruments = instruments.Where(i => i.Brand?.Name.Contains(brand, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrEmpty(price))
                instruments = price.ToLower() switch
                {
                    "low" => instruments.Where(i => i.Price < 200),
                    "mid" => instruments.Where(i => i.Price >= 200 && i.Price <= 1000),
                    "high" => instruments.Where(i => i.Price > 1000),
                    _ => instruments
                };

            int totalItems = instruments.Count();

            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (page < 1)
                page = 1;
            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var itemsOnPage = instruments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InstrumentViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    BrandName = i.Brand?.Name ?? "Unknown",
                    ImageUrl = i.Images.OrderByDescending(img => img.IsMain).FirstOrDefault()?.Url ?? "/images/default-image.png"
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(itemsOnPage);
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
                    ? item.Images.OrderByDescending(img=>img.IsMain).Select(img => img.Url).ToList()
                    : new List<string> { "/images/default-image.png" }
            };
            return View(viewModel);
        }
    }
}