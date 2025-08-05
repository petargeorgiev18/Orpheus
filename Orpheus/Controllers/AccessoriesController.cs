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
    public class AccessoriesController : Controller
    {
        private readonly IAccessoryService accessoryService;
        private readonly IReviewService reviewService;
        private readonly IRepository<Brand, Guid> brandRepo;

        public AccessoriesController(
            IAccessoryService accessoryService,
            IReviewService reviewService,
            IRepository<Brand, Guid> brandRepo)
        {
            this.accessoryService = accessoryService;
            this.reviewService = reviewService;
            this.brandRepo = brandRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? sort, string? price, int page = 1, int pageSize = 6)
        {
            var accessories = await accessoryService.GetAvailableAccessoriesAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                accessories = accessories
                    .Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(price))
            {
                accessories = price.ToLower() switch
                {
                    "low" => accessories.Where(i => i.Price < 20),
                    "mid" => accessories.Where(i => i.Price >= 20 && i.Price <= 60),
                    "high" => accessories.Where(i => i.Price > 60),
                    _ => accessories
                };
            }

            accessories = sort?.ToLower() switch
            {
                "priceasc" => accessories.OrderBy(i => i.Price),
                "pricedesc" => accessories.OrderByDescending(i => i.Price),
                _ => accessories.OrderBy(i => i.Name)
            };

            int totalItems = accessories.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            page = Math.Clamp(page, 1, totalPages > 0 ? totalPages : 1);

            var itemsOnPage = accessories
                .OrderBy(i=>i.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new ItemViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Price = i.Price,
                    BrandName = i.Brand?.Name ?? "Unknown",
                    ImageUrl = i.Images
                        .OrderByDescending(img => img.IsMain)
                        .Select(img => img.Url)
                        .FirstOrDefault() ?? "/images/default-image.png"
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
            var item = await accessoryService.GetByIdAsync(id);

            if (item == null || item.ItemType != ItemType.Accessory || !item.IsAvailable)
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
                BrandName = item.Brand?.Name ?? "Unknown",
                Images = item.Images
                    .OrderByDescending(i => i.IsMain)
                    .Select(i => i.Url)
                    .ToList(),
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
            vm.ItemType = ItemType.Accessory;
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

            model.ItemType = ItemType.Accessory;

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

            await accessoryService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var accessory = await accessoryService.GetByIdAsync(id);
            if (accessory == null) return NotFound();

            var vm = new CreateEditItemViewModel
            {
                Id = accessory.Id,
                Name = accessory.Name,
                Description = accessory.Description,
                Price = accessory.Price,
                BrandId = accessory.BrandId,
                ItemType = ItemType.Accessory
            };

            await PopulateDropdownsAsync(vm);
            return View("Edit", vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateEditItemViewModel model)
        {
            model.ItemType = ItemType.Accessory;

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

            await accessoryService.UpdateAsync(dto);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accessory = await accessoryService.GetByIdAsync(id);
            if (accessory == null) return NotFound();

            var vm = new ItemViewModel
            {
                Id = accessory.Id,
                Name = accessory.Name,
                BrandName = accessory.Brand?.Name ?? "Unknown",
                Price = accessory.Price,
                ImageUrl = accessory.Images.FirstOrDefault()?.Url ?? "/images/default-image.png"
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            await accessoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

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