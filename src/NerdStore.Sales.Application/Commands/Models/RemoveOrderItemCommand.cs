using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class RemoveOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public RemoveOrderItemCommand(Guid customerId, Guid orderId, Guid productId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveOrderItemCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
