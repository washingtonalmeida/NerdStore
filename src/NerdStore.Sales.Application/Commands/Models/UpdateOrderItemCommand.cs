using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class UpdateOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int ProductQuantity { get; private set; }

        public UpdateOrderItemCommand(Guid customerId, Guid orderId, Guid productId, int productQuantity)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductQuantity = productQuantity;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderItemCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
