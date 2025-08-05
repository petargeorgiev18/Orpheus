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
    public class MerchController : Controller
    {
        private readonly IMerchService merchService;
        private readonly IReviewService reviewService;
        private readonly IRepository<Brand, Guid> brandRepo;

        public MerchController(IMerchService merchService,
            IReviewService reviewService, IRepository<Brand, Guid> brandRepo)
        {
            this.merchService = merchService;
            this.reviewService = reviewService;
            this.brandRepo = brandRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, string? sort, string? price, int page = 1, int pageSize = 6)
        {
            var merch = await merchService.GetAvailableMerchAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                merch = merch.Where(i => i.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                    || (i.Brand != null && i.Brand.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(price))
            {
                price = price.ToLower();
                merch = price switch
                {
                    "low" => merch.Where(i => i.Price < 20),
                    "mid" => merch.Where(i => i.Price >= 20 && i.Price <= 60),
                    "high" => merch.Where(i => i.Price > 60),
                    _ => merch
                };
            }

            sort = sort?.ToLower();
            merch = sort switch
            {
                "priceasc" => merch.OrderBy(i => i.Price),
                "pricedesc" => merch.OrderByDescending(i => i.Price),
                _ => merch.OrderBy(i => i.Name)
            };

            int totalItems = merch.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            page = Math.Clamp(page, 1, totalPages > 0 ? totalPages : 1);

            var itemsOnPage = merch
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
            var item = await merchService.GetByIdAsync(id);

            if (item == null || item.ItemType != ItemType.Merch || !item.IsAvailable)
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
                Images = item.Images.OrderByDescending(i => i.IsMain).Select(i => i.Url).ToList(),
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
            vm.ItemType = ItemType.Merch;
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
            model.ItemType = ItemType.Merch;

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

            await merchService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var merch = await merchService.GetByIdAsync(id);
            if (merch == null) return NotFound();

            var vm = new CreateEditItemViewModel
            {
                Id = merch.Id,
                Name = merch.Name,
                Description = merch.Description,
                Price = merch.Price,
                BrandId = merch.BrandId,
                ItemType = ItemType.Merch
            };

            await PopulateDropdownsAsync(vm);
            return View("Edit", vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateEditItemViewModel model)
        {
            model.ItemType = ItemType.Merch;

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

            await merchService.UpdateAsync(dto);
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var merch = await merchService.GetByIdAsync(id);
            if (merch == null) return NotFound();

            var vm = new ItemViewModel
            {
                Id = merch.Id,
                Name = merch.Name,
                BrandName = merch.Brand?.Name ?? "Unknown",
                Price = merch.Price,
                ImageUrl = merch.Images.FirstOrDefault()?.Url ?? "/images/default-image.png"
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            await merchService.DeleteAsync(id);
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