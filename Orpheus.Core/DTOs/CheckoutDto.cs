using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus.Core.DTOs
{
    public class CheckoutDto
    {
        public Guid UserId { get; set; }
        public List<CheckoutItemDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string FullName { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string PaymentMethod { get; set; } = "CreditCard";
        public string? CardNumber { get; set; }
        public int? ExpiryMonth { get; set; }
        public int? ExpiryYear { get; set; }
        public string? CVV { get; set; }
    }
}
