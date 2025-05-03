using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orpheus.Data.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        [ForeignKey(nameof(Category))]  
        public int CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
        public ICollection<ItemImage>? Images { get; set; } 
            = new List<ItemImage>();
        public ICollection<Review>? Reviews { get; set; }
            = new List<Review>();
        public bool IsAvailable { get; set; } = true;
    }
}