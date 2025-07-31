using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orpheus.Core.DTOs;
using Orpheus.Data.Models;

namespace Orpheus.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(CheckoutDto model);
        Task<List<OrderDto>> GetOrdersByUserAsync(Guid userId);

    }
}
