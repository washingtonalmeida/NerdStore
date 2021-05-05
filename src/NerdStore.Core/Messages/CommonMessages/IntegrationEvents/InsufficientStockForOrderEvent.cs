using System;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class InsufficientStockForOrderEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public InsufficientStockForOrderEvent(Guid customerId, Guid orderId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}
