﻿using FluentValidation;
using NerdStore.Sales.Application.Commands.Models;
using System;

namespace NerdStore.Sales.Application.Commands.Validations
{
    public class CancelOrderProcessingAndSupplyStockCommandValidation : AbstractValidator<CancelOrderProcessingAndSupplyStockCommand>
    {
        public CancelOrderProcessingAndSupplyStockCommandValidation()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("The customer id is invalid.");

            RuleFor(x => x.OrderId)
                .NotEqual(Guid.Empty)
                .WithMessage("The order id is invalid.");
        }
    }
}
