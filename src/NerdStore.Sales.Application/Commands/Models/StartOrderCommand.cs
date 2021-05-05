using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Commands.Validations;
using System;

namespace NerdStore.Sales.Application.Commands.Models
{
    public class StartOrderCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string CardName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpirationDate { get; private set; }
        public string CardCvv { get; private set; }

        public StartOrderCommand(Guid customerId, Guid orderId, decimal totalPrice, string cardName, string cardNumber, string cardExpirationDate, string cardCvv)
        {
            CustomerId = customerId;
            OrderId = orderId;
            TotalPrice = totalPrice;
            CardName = cardName;
            CardNumber = cardNumber;
            CardExpirationDate = cardExpirationDate;
            CardCvv = cardCvv;
        }

        public override bool IsValid()
        {
            ValidationResult = new StartOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
