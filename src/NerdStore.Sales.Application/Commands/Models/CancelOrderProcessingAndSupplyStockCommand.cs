using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class CancelOrderProcessingAndSupplyStockCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public CancelOrderProcessingAndSupplyStockCommand(Guid customerId, Guid orderId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CancelOrderProcessingAndSupplyStockCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
