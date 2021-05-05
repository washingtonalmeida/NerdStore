using NerdStore.Core.DomainObjects.DTO;
using System;

namespace NerdStore.Core.Messages.CommonMessages.IntegrationEvents
{
    public class StockReservedForOrderEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpirationDate { get; private set; }
        public string CardCvv { get; private set; }
        public OrderProductsDto OrderProducts { get; private set; }

        public StockReservedForOrderEvent(Guid customerId, Guid orderId, decimal totalPrice, string cardName, string cardNumber, string cardExpirationDate, string cardCvv, OrderProductsDto orderProducts)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            TotalPrice = totalPrice;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpirationDate = cardExpirationDate;
            CardCvv = cardCvv;
            OrderProducts = orderProducts;
        }
    }
}
