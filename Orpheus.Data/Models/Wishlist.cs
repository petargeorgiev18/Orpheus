using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orpheus.Data.Models
{
    public class Wishlist
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public OrpheusAppUser User { get; set; } = null!;
        public ICollection<WishlistItem> WishlistsItems { get; set; } 
            = new List<WishlistItem>();
    }
}