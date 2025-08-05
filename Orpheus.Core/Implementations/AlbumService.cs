using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class AlbumService : IAlbumService
    {
        private readonly IRepository<Item, Guid> itemRepo;

        public AlbumService(IRepository<Item, Guid> itemRepo)
        {
            this.itemRepo = itemRepo;
        }

        public async Task<IEnumerable<Item>> GetAvailableAlbumsAsync()
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Where(i => i.ItemType == ItemType.Album && i.IsAvailable)
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
            var album = new Item
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                ItemType = ItemType.Album,
                IsAvailable = true,
                Images = model.ImageUrls?.Select(url => new ItemImage
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    IsMain = url == model.ImageUrls.First()
                }).ToList() ?? new List<ItemImage>()
            };

            await itemRepo.AddAsync(album);
        }

        public async Task UpdateAsync(CreateEditItemDto model)
        {
            var album = await itemRepo.GetAllTracked().FirstOrDefaultAsync(i => i.Id == model.Id);

            if (album == null) throw new Exception("Album not found");

            album.Name = model.Name;
            album.Description = model.Description;
            album.Price = model.Price;
            album.BrandId = model.BrandId;
            album.ItemType = model.ItemType;

            await itemRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var album = await itemRepo.GetByIdAsync(id);
            if (album == null) throw new Exception("Album not found");

            await itemRepo.DeleteAsync(id);
        }

    }
}
