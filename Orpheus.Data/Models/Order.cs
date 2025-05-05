using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orpheus.Data.Models.Enums;

namespace Orpheus.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public OrdersStatus OrderStatus { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        public PaymentStatus PaymentStatus { get; set; }
        [Required]
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public OrpheusAppUser User { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}
