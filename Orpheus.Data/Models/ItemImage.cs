using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orpheus.Data.Models
{
    public class ItemImage
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Url { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Item))]
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}
