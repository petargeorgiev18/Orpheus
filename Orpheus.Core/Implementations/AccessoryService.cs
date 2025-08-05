using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class AccessoryService : IAccessoryService
    {
        private readonly IRepository<Item, Guid> itemRepo;

        public AccessoryService(IRepository<Item, Guid> itemRepo)
        {
            this.itemRepo = itemRepo;
        }

        public async Task<IEnumerable<Item>> GetAvailableAccessoriesAsync()
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Where(i => i.ItemType == ItemType.Accessory && i.IsAvailable)
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

        public async Task CreateAsync(CreateEditItemDto model)
        {
            var accessory = new Item
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                ItemType = ItemType.Accessory,
                IsAvailable = true,
                Images = model.ImageUrls?.Select(url => new ItemImage
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    IsMain = url == model.ImageUrls.First()
                }).ToList() ?? new List<ItemImage>()
            };

            await itemRepo.AddAsync(accessory);
        }

        public async Task UpdateAsync(CreateEditItemDto model)
        {
            var accessory = await itemRepo.GetAllTracked().FirstOrDefaultAsync(i => i.Id == model.Id);

            if (accessory == null)
                throw new Exception("Accessory not found");

            accessory.Name = model.Name;
            accessory.Description = model.Description;
            accessory.Price = model.Price;
            accessory.BrandId = model.BrandId;
            accessory.ItemType = ItemType.Accessory;

            await itemRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var accessory = await itemRepo.GetByIdAsync(id);
            if (accessory == null)
                throw new Exception("Accessory not found");

            await itemRepo.DeleteAsync(id);
        }
    }
}