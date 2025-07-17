using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class InstrumentItemController : Controller
    {
        private readonly IInstrumentItemService instrumentItemService;

        public InstrumentItemController(IInstrumentItemService instrumentItemService)
        {
            this.instrumentItemService = instrumentItemService;
        }
        public async Task<IActionResult> All()
        {
            var instruments = await instrumentItemService.GetAvailableInstrumentsAsync();

            var viewModel = instruments.Select(i => new InstrumentItemViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                BrandName = i.Brand.Name,
                ImageUrl = i.Images.FirstOrDefault()?.Url ?? "/img/default.png"
            });
            return View(viewModel);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            // Load the instrument by id, then return the details view
            var instrument = await instrumentItemService.GetByIdAsync(id);
            if (instrument == null)
                return NotFound();

            return View(instrument);
        }
    }
}
