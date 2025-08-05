using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class MerchService : IMerchService
    {
        private readonly IRepository<Item, Guid> itemRepo;

        public MerchService(IRepository<Item, Guid> itemRepo)
        {
            this.itemRepo = itemRepo;
        }

        public async Task<IEnumerable<Item>> GetAvailableMerchAsync()
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Where(i => i.ItemType == ItemType.Merch && i.IsAvailable)
                .Include(i => i.Category)
                .Include(i => i.Images)
                .Include(i => i.Brand)
                .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.Images)
                .Include(i => i.Brand)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task CreateAsync(CreateEditItemDto model)
        {
            var merch = new Item
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                ItemType = ItemType.Merch,
                IsAvailable = true,
                Images = model.ImageUrls?.Select(url => new ItemImage
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    IsMain = url == model.ImageUrls.First()
                }).ToList() ?? new List<ItemImage>()
            };

            await itemRepo.AddAsync(merch);
        }

        public async Task UpdateAsync(CreateEditItemDto model)
        {
            var merch = await itemRepo.GetAllTracked().FirstOrDefaultAsync(i => i.Id == model.Id);

            if (merch == null)
                throw new Exception("Merch item not found");

            merch.Name = model.Name;
            merch.Description = model.Description;
            merch.Price = model.Price;
            merch.BrandId = model.BrandId;
            merch.ItemType = ItemType.Merch;

            await itemRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var merch = await itemRepo.GetByIdAsync(id);
            if (merch == null)
                throw new Exception("Merch item not found");

            await itemRepo.DeleteAsync(id);
        }
    }
}