using Microsoft.AspNetCore.Mvc;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models.Enums;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;

        public AlbumsController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? sort, int page = 1, int pageSize = 6)
        {
            var albums = await albumService.GetAvailableAlbumsAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                albums = albums
                    .Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                albums = sort.ToLower() switch
                {
                    "priceasc" => albums.OrderBy(i => i.Price),
                    "pricedesc" => albums.OrderByDescending(i => i.Price),
                    _ => albums
                };
            }

            int totalItems = albums.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Clamp page number to valid range
            if (page < 1)
                page = 1;
            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var itemsOnPage = albums
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InstrumentViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    BrandName = i.Brand?.Name ?? "Unknown",
                    ImageUrl = i.Images.OrderByDescending(img => img.IsMain)
                        .FirstOrDefault()?.Url ?? "/images/default-image.png"
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm ?? "";
            ViewBag.Sort = sort ?? "";

            return View(itemsOnPage);
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var item = await albumService.GetByIdAsync(id);

            if (item == null || item.ItemType != ItemType.Album || !item.IsAvailable)
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
                Images = item.Images.OrderByDescending(img => img.IsMain)
                    .Select(i => i.Url).ToList()
            };

            return View(viewModel);
        }
    }
}
