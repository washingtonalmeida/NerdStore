using FluentValidation;
using NerdStore.Sales.Application.Commands.Models;
using System;

namespace NerdStore.Sales.Application.Commands.Validations
{
    public class ApplyVoucherToOrderCommandValidation : AbstractValidator<ApplyVoucherToOrderCommand>
    {
        public ApplyVoucherToOrderCommandValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("The customer id is invalid.");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("The order id is invalid.");

            RuleFor(x => x.VoucherCode)
                .NotEmpty()
                .WithMessage("The voucher cannot be empty.");
        }
    }
}
