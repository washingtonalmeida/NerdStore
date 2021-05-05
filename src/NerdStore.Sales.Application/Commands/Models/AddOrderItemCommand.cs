using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class AddOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int ProductQuantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public AddOrderItemCommand(Guid customerId, Guid productId, string productName, int productQuantity, decimal unitPrice)
        {
            CustomerId = customerId;
            ProductId = productId;
            ProductName = productName;
            ProductQuantity = productQuantity;
            UnitPrice = unitPrice;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }

}
