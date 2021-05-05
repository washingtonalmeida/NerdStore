using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events.Models
{
    public class VoucherAppliedToOrderEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid VoucherId { get; private set ; }

        public VoucherAppliedToOrderEvent(Guid customerId, Guid orderId, Guid voucherId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            VoucherId = voucherId;
        }
    }
}
