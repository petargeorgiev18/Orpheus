namespace Orpheus.ViewModels
{
    public class CartItemViewModel
    {
        public Guid CartItemId { get; set; }
        public Guid ItemId { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
