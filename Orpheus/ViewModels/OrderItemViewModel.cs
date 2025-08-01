namespace Orpheus.ViewModels
{
    public class OrderItemViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? MaskedCardNumber { get; set; }
    }
}