using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus.Core.DTOs
{
    public class CheckoutItemDto
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
