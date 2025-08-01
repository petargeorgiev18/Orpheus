using System.ComponentModel.DataAnnotations;

namespace Orpheus.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new();

        public decimal GrandTotal => CartItems.Sum(i => i.TotalPrice);

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100)]
        [MinLength(15, ErrorMessage = "Full name must be at least 15 characters long.")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(150)]
        [MinLength(10, ErrorMessage = "Address must be at least 10 characters long.")]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(80)]
        [MinLength(3, ErrorMessage = "City must be at least 3 characters long.")]
        [RegularExpression(@"^[A-ZА-Я][a-zа-яA-ZА-Я\s\-]*$", ErrorMessage = "City must start with a capital letter and contain only letters.")]
        public string City { get; set; } = "";

        [Required(ErrorMessage = "Postal code is required.")]
        [RegularExpression(@"^\d{4,10}$", ErrorMessage = "Invalid postal code.")]
        [MaxLength(10, ErrorMessage = "Postal code cannot exceed 10 characters.")]
        [MinLength(4, ErrorMessage = "Postal code must be at least 4 characters long.")]
        public string PostalCode { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [MinLength(10, ErrorMessage = "Phone number must be at least 10 characters long.")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = "CreditCard";

        [Required(ErrorMessage = "CardNumber field is required.")]
        [Display(Name = "Card Number")]
        [CreditCard(ErrorMessage = "Invalid card number.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits.")]
        public string? CardNumber { get; set; }

        [Required(ErrorMessage = "ExpiryMonth field is required.")]
        [Display(Name = "Expiry Month")]
        [Range(1, 12, ErrorMessage = "Enter valid month (1-12).")]
        public int? ExpiryMonth { get; set; }

        [Required(ErrorMessage = "ExpiryYear field is required.")]
        [Display(Name = "Expiry Year")]
        [Range(2025, 2100, ErrorMessage = "Enter valid year.")]
        public int? ExpiryYear { get; set; }

        [Required(ErrorMessage = "CVV field is required.")]
        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
        public string? CVV { get; set; }
    }
}