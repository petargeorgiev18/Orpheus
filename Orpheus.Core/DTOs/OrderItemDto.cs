namespace Orpheus.Core.DTOs
{
    public class OrderItemDto
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}