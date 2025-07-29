using System.ComponentModel.DataAnnotations;

namespace Orpheus.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new();

        public decimal GrandTotal => CartItems.Sum(i => i.TotalPrice);

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        public string Address { get; set; } = "";

        [Required]
        public string City { get; set; } = "";

        [Required]
        [RegularExpression(@"^\d{4,10}$", ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; } = "";

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = "";

        [Required]
        public string PaymentMethod { get; set; } = "CreditCard";
    }
}