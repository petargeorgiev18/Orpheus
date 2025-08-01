namespace Orpheus.ViewModels
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
        public string? PaymentMethod { get; set; }
        public string? CardNumber { get; set; }
    }
}