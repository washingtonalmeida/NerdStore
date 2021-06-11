using NerdStore.Core.DomainObjects.DTO;
using System;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public  class OrderProcessingCanceledEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public OrderProductsDto OrderProducts { get; private set ; }

        public OrderProcessingCanceledEvent(Guid customerId, Guid orderId, OrderProductsDto orderProducts)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            OrderProducts = orderProducts;
        }
    }
}
