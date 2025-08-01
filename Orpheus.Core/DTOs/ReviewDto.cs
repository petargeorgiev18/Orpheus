namespace Orpheus.Core.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ItemId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = null!;

    }
}
