using FluentValidation;
using NerdStore.Sales.Application.Commands.Models;
using System;

namespace NerdStore.Sales.Application.Commands.Validations
{
    public class StartOrderCommandValidation : AbstractValidator<StartOrderCommand>
    {
        public StartOrderCommandValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("The customer id is invalid.");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("The order id is invalid.");

            RuleFor(x => x.TotalPrice)
                .NotEmpty()
                .WithMessage("The total price is invalid.");

            RuleFor(x => x.CardName)
                .NotEmpty()
                .WithMessage("The card name is invalid.");

            RuleFor(x => x.CardNumber)
                .CreditCard()
                .WithMessage("The card number is invalid.");

            RuleFor(x => x.CardExpirationDate)
                .NotEmpty()
                .WithMessage("The card expiration date is invalid.");

            RuleFor(x => x.CardCvv)
                .Length(3, 4)
                .WithMessage("The card CVV is invalid.");
        }
    }
}
