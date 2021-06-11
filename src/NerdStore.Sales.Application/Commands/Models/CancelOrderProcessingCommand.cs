using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class CancelOrderProcessingCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public CancelOrderProcessingCommand(Guid customerId, Guid orderId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CancelOrderProcessingCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
