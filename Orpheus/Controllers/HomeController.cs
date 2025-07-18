using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.Models;
using Orpheus.ViewModels;
using System.Diagnostics;

namespace Orpheus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IInstrumentService instrumentService;

        public HomeController(
            ILogger<HomeController> logger,
            IInstrumentService instrumentService)
        {
            _logger = logger;
            this.instrumentService = instrumentService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var featuredInstruments = await instrumentService.GetFeaturedInstrumentsAsync(3);
            var viewModels = featuredInstruments.Select(i => new InstrumentViewModel
            {
                Id = i.Id,
                Name = i.Name,
                BrandName = i.Brand.Name,
                Description = i.Description,
                Price = i.Price,
                Images = i.Images.Select(img => img.Url).ToList()
            });
            return View(viewModels);
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
