using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Implementations;
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
        public async Task<IActionResult> All()
        {
            var instruments = await instrumentItemService.GetAvailableInstrumentsAsync();

            var viewModel = instruments.Select(i => new InstrumentViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                BrandName = i.Brand.Name,
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
