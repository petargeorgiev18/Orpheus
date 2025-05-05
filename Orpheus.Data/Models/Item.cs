using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Orpheus.Common.EntityClassesValidation.Item;
namespace Orpheus.Data.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(ItemNameMaxLength)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(ItemDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        [ForeignKey(nameof(Category))]  
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Brand))]
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public ICollection<ItemImage> Images { get; set; } 
            = new List<ItemImage>();
        public ICollection<Review>? Reviews { get; set; }
            = new List<Review>();
        public bool IsAvailable { get; set; } = true;
    }
}