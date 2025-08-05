using System.ComponentModel.DataAnnotations;
using Orpheus.Data.Models.Enums;

namespace Orpheus.Core.DTOs
{
    public class CreateEditItemDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Range(0, double.MaxValue)]
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
