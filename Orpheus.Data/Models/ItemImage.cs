using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orpheus.Data.Models
{
    public class ItemImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required]
        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}