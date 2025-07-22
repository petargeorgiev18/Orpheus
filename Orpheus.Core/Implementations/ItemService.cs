using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item, Guid> itemRepo;

        public ItemService(IRepository<Item, Guid> itemRepo)
        {
            this.itemRepo = itemRepo;
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Include(i => i.Category)
                .Include(i => i.Brand)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id && i.IsAvailable);
        }

        public async Task<IEnumerable<Item>> GetAvailableItemsAsync()
        {
            return await itemRepo
                .GetAllAsNoTracking()
                .Where(i => i.IsAvailable)
                .Include(i => i.Category)
                .Include(i => i.Brand)
                .Include(i => i.Images)
                .ToListAsync();
        }
    }

}
