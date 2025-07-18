using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IRepository<Item, Guid> itemRepo;
        public InstrumentService(IRepository<Item, Guid> itemRepo)
        {
            this.itemRepo = itemRepo;
        }
        public async Task<IEnumerable<Item>> GetAvailableInstrumentsAsync()
        {
            return await itemRepo                
                .GetAllAsNoTracking()
                .Where(i => i.ItemType == ItemType.Instrument && i.IsAvailable)
                .Include(i=>i.Images)
                .Include(i => i.Brand)
                .ToListAsync();
        }
        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await itemRepo.GetAllAsNoTracking()
                    .Include(i => i.Images)
                    .Include(i => i.Brand)
                    .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
