using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class ApplyVoucherToOrderCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public string VoucherCode { get; private set; }

        public ApplyVoucherToOrderCommand(Guid customerId, Guid orderId, string voucherCode)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            VoucherCode = voucherCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new ApplyVoucherToOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
