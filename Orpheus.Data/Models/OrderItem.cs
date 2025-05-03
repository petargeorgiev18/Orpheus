using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Orpheus.Data.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ItemId))]
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}