using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;
using Orpheus.ViewModels;

namespace Orpheus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly IRepository<Category, Guid> categoryRepo;

        public CategoriesController(IRepository<Category, Guid> categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = model.CategoryName
            };

            await categoryRepo.AddAsync(newCategory);

            return RedirectToAction("All", "Instruments");
        }
    }
}