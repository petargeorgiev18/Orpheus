using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Orpheus.Common.EntityClassesValidation.Review;

namespace Orpheus.Data.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(ReviewCommentMaxLength)]
        public string? Comment { get; set; } = string.Empty;
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsDeleted { get; set; } = false;
        [Required]
        [ForeignKey(nameof(Item))]
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public OrpheusAppUser User { get; set; } = null!;
    }
}