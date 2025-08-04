using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Orpheus.Common.EntityClassesValidation.Item;

namespace Orpheus.ViewModels
{
    public class CreateEditInstrumentViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ItemNameMaxLength, ErrorMessage = "Item name can't be longer than 100 characters")]
        [MinLength(ItemNameMinLength, ErrorMessage = "Item name can't be shorter than 3 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(ItemDescriptionMaxLength, ErrorMessage = "Item description can't be longer than 400 characters")]
        [MinLength(5, ErrorMessage = "Item description can't be shorter than 5 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(ItemPriceMinValue, ItemPriceMaxValue, ErrorMessage = "Invalid item price" )]
        public decimal Price { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public ItemType ItemType { get; set; }

        public List<string>? ImageUrls { get; set; }
    }
}
