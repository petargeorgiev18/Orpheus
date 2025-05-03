using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus.Data.Models.Enums
{
    public enum OrdersStatus
    {
        Pending = 1,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
