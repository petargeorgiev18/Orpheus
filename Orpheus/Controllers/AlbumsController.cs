using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumService albumService;
        private readonly IReviewService reviewService;
        private readonly IRepository<Brand, Guid> brandRepo;

        public AlbumsController(
            IAlbumService albumService,
            IReviewService reviewService,
            IRepository<Brand, Guid> brandRepo)
        {
            this.albumService = albumService;
            this.reviewService = reviewService;
            this.brandRepo = brandRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? sort, string? price, int page = 1, int pageSize = 6)
        {
            var albums = await albumService.GetAvailableAlbumsAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                albums = albums.Where(i =>
                    i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(price))
            {
                albums = price.ToLower() switch
                {
                    "low" => albums.Where(i => i.Price < 20m),
                    "mid" => albums.Where(i => i.Price >= 20m && i.Price <= 60m),
                    "high" => albums.Where(i => i.Price > 60m),
                    _ => albums
                };
            }

            if (!string.IsNullOrWhiteSpace(sort))
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

            if (page < 1)
                page = 1;
            if (totalPages > 0 && page > totalPages)
                page = totalPages;

            var itemsOnPage = albums
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new ItemViewModel
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
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm ?? "";
            ViewBag.Sort = sort ?? "";
            ViewBag.Price = price ?? "";

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

            var reviews = await reviewService.GetReviewsByItemIdAsync(item.Id);

            var viewModel = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                BrandName = item.Brand?.Name ?? "Unknown Brand",
                Images = item.Images.OrderByDescending(img => img.IsMain).Select(i => i.Url).ToList(),
                Reviews = reviews.Select(r => new ReviewViewModel
                {
                    UserFullName = r.UserFullName!,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditItemViewModel();
            await PopulateDropdownsAsync(vm);
            return View("Create", vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEditItemViewModel model)
        {
            var urls = (model.ImageUrlsRaw ?? "")
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(u => u.Trim())
                .Where(u => !string.IsNullOrEmpty(u))
                .ToList();

            if (!urls.Any()) urls.Add("/images/default-image.png");
            model.ImageUrls = urls;

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model);
                return View("Create", model);
            }

            var dto = new CreateEditItemDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                ItemType = model.ItemType,
                ImageUrls = model.ImageUrls
            };

            await albumService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var album = await albumService.GetByIdAsync(id);
            if (album == null) return NotFound();

            var vm = new CreateEditItemViewModel
            {
                Id = album.Id,
                Name = album.Name,
                Description = album.Description,
                Price = album.Price,
                BrandId = album.BrandId,
                ItemType = album.ItemType
            };

            await PopulateDropdownsAsync(vm);
            return View("Edit", vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateEditItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model);
                return View("Edit", model);
            }

            var dto = new CreateEditItemDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                ItemType = model.ItemType
            };

            await albumService.UpdateAsync(dto);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var album = await albumService.GetByIdAsync(id);
            if (album == null) return NotFound();

            var vm = new ItemViewModel
            {
                Id = album.Id,
                Name = album.Name,
                BrandName = album.Brand?.Name ?? "Unknown",
                Price = album.Price,
                ImageUrl = album.Images.FirstOrDefault()?.Url ?? "/images/default-image.png"
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            await albumService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Helper method
        private async Task PopulateDropdownsAsync(CreateEditItemViewModel model)
        {
            var brands = await brandRepo.GetAllAsNoTracking().ToListAsync();

            model.Brands = brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });
        }

    }
}
