using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orpheus.Data.Models;

namespace Orpheus.Core.Interfaces
{
    public interface IAccessoryService
    {
        Task<IEnumerable<Item>> GetAvailableAccessoriesAsync();
        Task<Item?> GetByIdAsync(Guid id);
    }
}
