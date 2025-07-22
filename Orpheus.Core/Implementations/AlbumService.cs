using Microsoft.EntityFrameworkCore;
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
    }
}
