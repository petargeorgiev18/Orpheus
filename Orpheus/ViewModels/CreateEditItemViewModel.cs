using Microsoft.AspNetCore.Mvc.Rendering;
using Orpheus.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Orpheus.Common.EntityClassesValidation.Item;

namespace Orpheus.ViewModels
{
    public class CreateEditItemViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ItemNameMaxLength)]
        [MinLength(ItemNameMinLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(ItemDescriptionMaxLength)]
        [MinLength(5)]
        public string Description { get; set; } = string.Empty;

        [Range(ItemPriceMinValue, ItemPriceMaxValue)]
        public decimal Price { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        public ItemType ItemType { get; set; } = ItemType.Album;

        public List<string>? ImageUrls { get; set; }

        [Display(Name = "Image URLs (one per line)")]
        public string ImageUrlsRaw
        {
            get => string.Join(Environment.NewLine, ImageUrls ?? new List<string>());
            set => ImageUrls = string.IsNullOrWhiteSpace(value)
                ? new List<string>()
                : value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public IEnumerable<SelectListItem> Brands { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
