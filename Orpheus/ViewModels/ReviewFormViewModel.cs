using System.ComponentModel.DataAnnotations;
using static Orpheus.Common.EntityClassesValidation.Review;

namespace Orpheus.ViewModels
{
    public class ReviewFormViewModel
    {
        [Required]
        [Range(ReviewRatingMinValue, ReviewRatingMaxValue, ErrorMessage = "Please provide a rating between 1 and 5")]
        public int Rating { get; set; }

        [MaxLength(ReviewCommentMaxLength, ErrorMessage = "Comment should not exceed 120 characters")]
        public string? Comment { get; set; }

        [Required]
        public Guid ItemId { get; set; }
    }
}
