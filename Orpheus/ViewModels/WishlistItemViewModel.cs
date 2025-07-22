namespace Orpheus.ViewModels
{
    public class WishlistItemViewModel
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string? BrandName { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string? ImageUrl { get; set; }
        public List<string> Images { get; set; } = new();

        public decimal TotalPrice => Price * Quantity;
    }
}
