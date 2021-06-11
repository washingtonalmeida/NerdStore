using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events.Models
{
    public class OrderFinishedEvent : Event
    {
        public Guid OrderId { get; private set; }

        public OrderFinishedEvent(Guid orderId)
        {
            AggregateId = orderId;
            OrderId = orderId;
        }
    }
}
