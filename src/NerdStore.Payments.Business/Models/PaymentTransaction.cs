using NerdStore.Core.DomainObjects;
using NerdStore.Payments.Business.Enums;
using System;

namespace NerdStore.Payments.Business.Models
{
    public class PaymentTransaction : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid PaymentId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public PaymentTransactionStatus Status { get; private set; }

        // EF. Relationship
        public Payment Payment { get; private set; }
    }
}
