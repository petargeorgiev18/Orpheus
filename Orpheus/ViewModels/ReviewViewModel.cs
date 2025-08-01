namespace Orpheus.ViewModels
{
    public class ReviewViewModel
    {
        public string UserFullName { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
