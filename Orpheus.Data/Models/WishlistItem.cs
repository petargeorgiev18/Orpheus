using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orpheus.Data.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Wishlist))]
        public Guid WishlistId { get; set; }
        public Wishlist Wishlist { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Item))]
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;
        [Required]
        public int Quantity { get; set; } = 1;
    }
}