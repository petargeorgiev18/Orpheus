using Orpheus.Core.DTOs;
using Orpheus.Data.Models;

namespace Orpheus.Core.Interfaces
{
    public interface IInstrumentService
    {
        Task<IEnumerable<Item>> GetAvailableInstrumentsAsync();
        Task<Item?> GetByIdAsync(Guid id);
        Task<IEnumerable<Item>> GetFeaturedInstrumentsAsync(int count);
        Task CreateAsync(CreateEditItemDto model);
        Task UpdateAsync(CreateEditItemDto model);
        Task DeleteAsync(Guid id);
    }
}
