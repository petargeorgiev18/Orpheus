using Orpheus.Core.DTOs;
using Orpheus.Data.Models;

namespace Orpheus.Core.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Item>> GetAvailableAlbumsAsync();
        Task<Item?> GetByIdAsync(Guid id);
        Task CreateAsync(CreateEditItemDto model);
        Task UpdateAsync(CreateEditItemDto model);
        Task DeleteAsync(Guid id);
    }
}
