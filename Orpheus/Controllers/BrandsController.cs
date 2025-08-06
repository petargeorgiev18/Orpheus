using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BrandsController : Controller
    {
        private readonly IRepository<Brand, Guid> brandRepo;

        public BrandsController(IRepository<Brand, Guid> brandRepo)
        {
            this.brandRepo = brandRepo;
        }

        [HttpGet]
        public IActionResult Create(string type = "brand")
        {
            ViewData["Type"] = type.ToLower();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = model.Name.Trim()
            };

            await brandRepo.AddAsync(brand);

            return RedirectToAction("All", "Instruments");
        }
    }
}