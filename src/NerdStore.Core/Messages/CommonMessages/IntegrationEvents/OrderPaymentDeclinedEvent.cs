using System;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class OrderPaymentDeclinedEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid PaymentId { get; private set; }
        public Guid PaymentTransactionId { get; private set; }
        public decimal TotalPrice { get; private set; }

        public OrderPaymentDeclinedEvent(Guid customerId, Guid orderId, Guid paymentId, Guid paymentTransactionId, decimal totalPrice)
        {
            AggregateId = paymentId;
            CustomerId = customerId;
            OrderId = orderId;
            PaymentId = paymentId;
            PaymentTransactionId = paymentTransactionId;
            TotalPrice = totalPrice;
        }
    }
}
