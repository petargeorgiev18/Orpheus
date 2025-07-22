using Orpheus.Data.Models;

namespace Orpheus.Core.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Item>> GetAvailableAlbumsAsync();
        Task<Item?> GetByIdAsync(Guid id);
    }
}
