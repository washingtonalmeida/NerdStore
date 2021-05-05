using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events.Models
{
    public class OrderItemUpdatedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int ProductQuantity { get; private set; }

        public OrderItemUpdatedEvent(Guid customerId, Guid orderId, Guid productId, int productQuantity)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductQuantity = productQuantity;
        }
    }
}
