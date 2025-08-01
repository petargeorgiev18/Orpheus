using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Implementations;
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
        private readonly IAlbumService albumService;

        public HomeController(
            ILogger<HomeController> logger,
            IInstrumentService instrumentService,
            IAlbumService albumService)
        {
            _logger = logger;
            this.instrumentService = instrumentService;
            this.albumService = albumService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var instruments = await instrumentService.GetFeaturedInstrumentsAsync(6);
            var albums = await albumService.GetAvailableAlbumsAsync();

            var model = new HomeViewModel
            {
                FeaturedInstruments = instruments.Select(i => new ItemViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    BrandName = i.Brand?.Name ?? "Unknown",
                    ImageUrl = i.Images
                        .OrderByDescending(ii => ii.IsMain)
                        .ThenBy(ii => ii.Id)
                        .FirstOrDefault()?.Url ?? "/images/default-image.png",
                    Images = i.Images.Select(img => img.Url).ToList()
                }),
                FeaturedAlbums = albums.Select(a => new ItemViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    BrandName = a.Brand?.Name ?? "Unknown",
                    ImageUrl = a.Images
                        .OrderByDescending(i => i.IsMain)
                        .ThenBy(i => i.Id)
                        .FirstOrDefault()?.Url ?? "/images/default-image.png",
                    Images = a.Images.Select(img => img.Url).ToList()
                })
            };

            return View(model);
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
