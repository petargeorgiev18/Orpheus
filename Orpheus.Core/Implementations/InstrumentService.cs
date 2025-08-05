using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IRepository<Item, Guid> itemRepo;
        private readonly IRepository<ItemImage, Guid> itemImageRepo;
        public InstrumentService(IRepository<Item, Guid> itemRepo, IRepository<ItemImage, Guid> itemImageRepo)
        {
            this.itemRepo = itemRepo;
            this.itemImageRepo = itemImageRepo;
        }
        public async Task<IEnumerable<Item>> GetAvailableInstrumentsAsync()
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Where(i => i.ItemType == ItemType.Instrument && i.IsAvailable)
                .Include(i => i.Category)
                .Include(i => i.Images)
                .Include(i => i.Brand)
                .ToListAsync();
        }
        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await itemRepo.GetAllAsNoTracking()
                    .Include(i => i.Category)
                    .Include(i => i.Images)
                    .Include(i => i.Brand)
                    .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<IEnumerable<Item>> GetFeaturedInstrumentsAsync(int count)
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Where(i => i.ItemType == ItemType.Instrument && i.IsAvailable)
                .Include(i => i.Category)
                .Include(i => i.Images)
                .Include(i => i.Brand)
                .OrderBy(i => i.Reviews.Count)
                .Take(count)
                .ToListAsync();
        }
        public async Task CreateAsync(CreateEditItemDto model)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId,
                ItemType = ItemType.Instrument,
                IsAvailable = true,
                Images = model.ImageUrls?.Select(url => new ItemImage
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    IsMain = url == model.ImageUrls.First()
                }).ToList() ?? new List<ItemImage>()
            };

            await itemRepo.AddAsync(item);
        }

        public async Task UpdateAsync(CreateEditItemDto model)
        {
            var item = await itemRepo.GetAllTracked()
                .FirstOrDefaultAsync(i => i.Id == model.Id);

            if (item == null)
                throw new Exception("Item not found");

            item.Name = model.Name;
            item.Description = model.Description;
            item.Price = model.Price;
            item.BrandId = model.BrandId;
            item.CategoryId = model.CategoryId;
            item.ItemType = model.ItemType;           

            await itemRepo.SaveChangesAsync();
        }



        public async Task DeleteAsync(Guid id)
        {
            var item = await itemRepo.GetByIdAsync(id);
            if (item == null)
                throw new Exception("Item not found");

            await itemRepo.DeleteAsync(id);
        }
    }
}
