using System.ComponentModel.DataAnnotations;
using static Orpheus.Common.EntityClassesValidation.OrpheusAppUser;

namespace Orpheus.ViewModels
{
    public class CheckoutViewModel : IValidatableObject
    {
        public List<CartItemViewModel> CartItems { get; set; } = new();

        public decimal GrandTotal => CartItems.Sum(i => i.TotalPrice);

        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(FullNameMaxLength)]
        [MinLength(FullNameMinLength, ErrorMessage = "Full name must be at least 10 characters long.")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(AddressMaxLength)]
        [MinLength(AddressMinLength, ErrorMessage = "Address must be at least 10 characters long.")]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(CityMaxLength)]
        [MinLength(CityMinLength, ErrorMessage = "City must be at least 3 characters long.")]
        [RegularExpression(@"^[A-ZА-Я][a-zа-яA-ZА-Я\s\-]*$", ErrorMessage = "City must start with a capital letter and contain only letters.")]
        public string City { get; set; } = "";

        [Required(ErrorMessage = "Postal code is required.")]
        [RegularExpression(@"^\d{4,10}$", ErrorMessage = "Invalid postal code.")]
        [MaxLength(PostalCodeMaxLength, ErrorMessage = "Postal code cannot exceed 10 characters.")]
        [MinLength(PostalCodeMinLength, ErrorMessage = "Postal code must be at least 4 characters long.")]
        public string PostalCode { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [MaxLength(PhoneNumberMaxLength, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [MinLength(PhoneNumberMinLength, ErrorMessage = "Phone number must be at least 10 characters long.")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = "CreditCard";

        [Display(Name = "Card Number")]
        [CreditCard(ErrorMessage = "Invalid card number.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Card number must be 16 digits.")]
        public string? CardNumber { get; set; }

        [Display(Name = "Expiry Month")]
        [Range(ExpiryMonthMinValue, ExpiryMonthMaxValue, ErrorMessage = "Enter valid month (1-12).")]
        public int? ExpiryMonth { get; set; }

        [Display(Name = "Expiry Year")]
        [Range(ExpiryYearMinValue, ExpiryYearMaxValue, ErrorMessage = "Enter valid year.")]
        public int? ExpiryYear { get; set; }

        [Display(Name = "CVV")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits.")]
        public string? CVV { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaymentMethod == "CreditCard")
            {
                if (string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrEmpty(CardNumber))
                    yield return new ValidationResult("Card number is required.", new[] { nameof(CardNumber) });

                if (!ExpiryMonth.HasValue)
                    yield return new ValidationResult("Expiry month is required.", new[] { nameof(ExpiryMonth) });

                if (!ExpiryYear.HasValue)
                    yield return new ValidationResult("Expiry year is required.", new[] { nameof(ExpiryYear) });

                if (string.IsNullOrWhiteSpace(CVV) || string.IsNullOrEmpty(CVV))
                    yield return new ValidationResult("CVV is required.", new[] { nameof(CVV) });
            }
        }
    }
}