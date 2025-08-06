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
    public class InstrumentsController : Controller
    {
        private readonly IInstrumentService instrumentItemService;
        private readonly IReviewService reviewService;
        private readonly IRepository<Brand, Guid> brandRepo;
        private readonly IRepository<Category, Guid> categoryRepo;

        public InstrumentsController(
            IInstrumentService instrumentItemService,
            IReviewService reviewService,
            IRepository<Brand, Guid> brandRepo,
            IRepository<Category, Guid> categoryRepo)
        {
            this.instrumentItemService = instrumentItemService;
            this.reviewService = reviewService;
            this.brandRepo = brandRepo;
            this.categoryRepo = categoryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> All(string? searchTerm, string? category, string? brand, string? price, string? sort, int page = 1, int pageSize = 6)
        {
            var instruments = await instrumentItemService.GetAvailableInstrumentsAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                instruments = instruments.Where(i =>
                    i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    i.Brand?.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (!string.IsNullOrEmpty(category))
                instruments = instruments.Where(i => i.Category?.CategoryName.Contains(category, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrEmpty(brand))
                instruments = instruments.Where(i => i.Brand?.Name.Contains(brand, StringComparison.OrdinalIgnoreCase) == true);

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

            if (!string.IsNullOrEmpty(sort))
            {
                instruments = sort.ToLower() switch
                {
                    "priceasc" => instruments.OrderBy(i => i.Price),
                    "pricedesc" => instruments.OrderByDescending(i => i.Price),
                    _ => instruments
                };
            }

            int totalItems = instruments.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            page = Math.Clamp(page, 1, totalPages == 0 ? 1 : totalPages);

            var itemsOnPage = instruments
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
            ViewBag.Sort = sort;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Category = category;
            ViewBag.Brand = brand;
            ViewBag.Price = price;

            await PopulateDropdownsForFilteringAsync();

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

            var reviewsDto = await reviewService.GetReviewsByItemIdAsync(id);

            var reviewsVm = reviewsDto.Select(r => new ReviewViewModel
            {
                UserFullName = r.UserFullName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            var viewModel = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                BrandName = item.Brand?.Name ?? "Unknown Brand",
                Images = item.Images != null && item.Images.Any()
                    ? item.Images.OrderByDescending(img => img.IsMain).Select(img => img.Url).ToList()
                    : new List<string> { "/images/default-image.png" },
                Reviews = reviewsVm
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateEditInstrumentViewModel
            {
                ItemType = ItemType.Instrument 
            }; 
            await PopulateDropdownsAsync(viewModel);
            return View("Create", viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEditInstrumentViewModel model)
        {
            var urls = (model.ImageUrlsRaw ?? string.Empty)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(u => u.Trim())
                .Where(u => !string.IsNullOrEmpty(u))
                .ToList();

            if (!urls.Any())
            {
                urls.Add("/images/default-image.png");
            }

            model.ImageUrls = urls;

            if (string.IsNullOrWhiteSpace(model.ImageUrlsRaw))
            {
                ModelState.AddModelError(nameof(model.ImageUrlsRaw), "Image URLs are required.");
            }

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
                CategoryId = model.CategoryId,
                ItemType = model.ItemType,
                ImageUrls = model.ImageUrls
            };

            await instrumentItemService.CreateAsync(dto);
            return RedirectToAction(nameof(All));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await instrumentItemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var viewModel = new CreateEditInstrumentViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                BrandId = item.BrandId,
                CategoryId = item.Category.Id,
                ItemType = item.ItemType,
            };

            await PopulateDropdownsAsync(viewModel);
            return View("Edit", viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateEditInstrumentViewModel model)
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
                CategoryId = model.CategoryId,
                ItemType = model.ItemType,
            };

            await instrumentItemService.UpdateAsync(dto);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await instrumentItemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            var viewModel = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                BrandName = item.Brand?.Name ?? "Unknown",
                Price = item.Price,
                ImageUrl = item.Images.FirstOrDefault()?.Url ?? "/images/default-image.png"
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            await instrumentItemService.DeleteAsync(id);
            return RedirectToAction(nameof(All));
        }

        //Helper method to populate dropdowns for brands and categories
        private async Task PopulateDropdownsAsync(CreateEditInstrumentViewModel model)
        {
            var brands = await brandRepo.GetAllAsNoTracking().ToListAsync();
            var categories = await categoryRepo.GetAllAsNoTracking().ToListAsync();

            model.Brands = brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });

            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            });
        }

        //Helper method to populate dropdowns for brands and categories when filtering
        private async Task PopulateDropdownsForFilteringAsync()
        {
            var brands = await brandRepo.GetAllAsNoTracking().ToListAsync();
            var categories = await categoryRepo.GetAllAsNoTracking().ToListAsync();

            ViewBag.CategoriesList = categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryName,
                    Text = c.CategoryName
                });

            ViewBag.BrandsList = brands
                .Select(b => new SelectListItem
                {
                    Value = b.Name,
                    Text = b.Name
                });
        }
    }
}