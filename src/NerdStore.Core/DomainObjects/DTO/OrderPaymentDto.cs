using System;

namespace NerdStore.Core.DomainObjects.DTO
{
    public class OrderPaymentDto
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }
    }
}
