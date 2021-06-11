using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class FinishOrderCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public FinishOrderCommand(Guid customerId, Guid orderId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
        }

        public override bool IsValid()
        {
            ValidationResult = new FinishOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}