using FluentValidation;
using NerdStore.Sales.Application.Commands.Models;
using System;

namespace NerdStore.Sales.Application.Commands.Validations
{
    public class UpdateOrderItemCommandValidation : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemCommandValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("The customer id is invalid.");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("The order id is invalid.");

            RuleFor(x => x.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("The product id is invalid.");

            RuleFor(x => x.ProductQuantity)
                .GreaterThan(0)
                .WithMessage("The minimum product quantity is 1.");

            RuleFor(x => x.ProductQuantity)
                .LessThanOrEqualTo(15)
                .WithMessage("The maximum product quantity is 15. ");
        }
    }
}
