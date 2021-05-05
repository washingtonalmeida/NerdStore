using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events.Models
{
    public class OrderUpdatedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public decimal TotalPrice { get; private set; }

        public OrderUpdatedEvent(Guid customerId, Guid orderId, decimal totalPrice)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            TotalPrice = totalPrice;
        }
    }

 
}
