using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
using Orpheus.Data.Models.Enums;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Core.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order, Guid> orderRepository;
        private readonly IRepository<OrderItem, Guid> orderItemRepository;
        private readonly IRepository<Item, Guid> itemRepository;

        public OrderService(
            IRepository<Order, Guid> orderRepository,
            IRepository<OrderItem, Guid> orderItemRepository,
            IRepository<Item, Guid> itemRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.itemRepository = itemRepository;
        }

        public async Task<Guid> CreateOrderAsync(CheckoutDto model)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrdersStatus.Pending,
                PaymentStatus = PaymentStatus.Paid,
                Amount = model.TotalAmount,
                PaidAt = DateTime.MinValue,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                PostalCode = model.PostalCode,
                PhoneNumber = model.PhoneNumber,
                PaymentMethod = model.PaymentMethod,
                CardNumber = model.PaymentMethod == "CreditCard" && !string.IsNullOrEmpty(model.CardNumber)
        ? model.CardNumber[^4..] // last 4 digits
        : null,
                OrderItems = model.Items.Select(item => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                }).ToList()
            };


            await orderRepository.AddWithoutSavingAsync(order);
            await orderRepository.SaveChangesAsync();

            return order.Id;
        }



        public async Task<List<OrderDto>> GetOrdersByUserAsync(Guid userId)
        {
            var orders = await orderRepository.GetAllAsNoTracking()
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Status = order.OrderStatus.ToString(),
                TotalAmount = order.Amount,
                PaymentMethod = order.PaymentMethod,
                CardNumber = order.CardNumber,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Name = oi.Item.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }).ToList();
        }
    }
}