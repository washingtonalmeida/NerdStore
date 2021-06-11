using NerdStore.Core.DomainObjects;
using NerdStore.Sales.Domain.Enums;
using System;
using System.Collections.Generic;

namespace NerdStore.Sales.Domain.Models
{
    public class Voucher : Entity
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? MonetaryValue { get; private set; }
        public int Quantity { get; private set; }
        public VoucherDiscountType VoucherDiscountType { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        // Entity Framework Relationship
        public ICollection<Order> Orders { get; set; }

        protected Voucher() { }

        public Voucher(string code, decimal? percentage, decimal? monetaryValue, int quantity, VoucherDiscountType voucherDiscountType, DateTime registrationDate, DateTime expirationDate, bool active)
        {
            Code = code;
            Percentage = percentage;
            MonetaryValue = monetaryValue;
            Quantity = quantity;
            VoucherDiscountType = voucherDiscountType;
            RegistrationDate = registrationDate;
            ExpirationDate = expirationDate;
            Active = active;
        }

        public bool IsExpired()
        {
            return DateTime.Now >= ExpirationDate;
        }

        public bool HasQuantityAvailable()
        {
            return Quantity >= 1;
        }

        public bool IsUsed()
        {
            return Used;
        }

        public bool IsActive()
        {
            return Active;
        }
    }
}
