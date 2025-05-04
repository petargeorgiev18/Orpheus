using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orpheus.Data.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public string? Comment { get; set; } = string.Empty;
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsDeleted { get; set; } = false;
        [Required]
        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public OrpheusAppUser User { get; set; } = null!;
    }
}