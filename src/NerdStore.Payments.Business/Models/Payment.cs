using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Payments.Business.Models
{
    public class Payment : Entity, IAggregateRoot
    {
        public Guid OrderId { get; private set; }
        public string Status { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpirationDate { get; private set; }
        public string CardCvv { get; private set; }

        // EF. Relationship
        public PaymentTransaction PaymentTransaction { get; private set; }
    }
}
