using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orpheus.Data.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey(nameof(Cart))]
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Item))]
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;
        [Required]
        public int Quantity { get; set; } = 1;
    }
}