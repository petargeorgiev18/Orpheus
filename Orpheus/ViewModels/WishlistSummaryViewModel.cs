namespace Orpheus.ViewModels
{
    public class WishlistSummaryViewModel
    {
        public List<WishlistItemViewModel> Items { get; set; } = new();
        public decimal GrandTotal => Items.Sum(i => i.Price * i.Quantity);
    }
}
