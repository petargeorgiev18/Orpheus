using Microsoft.EntityFrameworkCore;
using Orpheus.Core.DTOs;
using Orpheus.Core.Interfaces;
using Orpheus.Data.Models;
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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var itemIds = model.Items.Select(i => i.ItemId).ToList();
            var itemsInDb = await itemRepository.GetAllAsNoTracking()
                .Where(i => itemIds.Contains(i.Id))
                .ToListAsync();

            if (itemsInDb.Count != model.Items.Count)
                throw new InvalidOperationException("Some items are no longer available.");

            foreach (var orderItem in model.Items)
            {
                var dbItem = itemsInDb.First(i => i.Id == orderItem.ItemId);
                if (dbItem.Price != orderItem.Price)
                    throw new InvalidOperationException($"Price mismatch for item {dbItem.Name}.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = Data.Models.Enums.OrdersStatus.Pending,
                PaymentStatus = Data.Models.Enums.PaymentStatus.Pending,
                Amount = model.TotalAmount,
                PaidAt = DateTime.MinValue 
            };

            await orderRepository.AddAsync(order);

            foreach (var item in model.Items)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                await orderItemRepository.AddAsync(orderItem);
            }

            return order.Id;
        }
    }
}