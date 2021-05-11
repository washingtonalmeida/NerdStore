using NerdStore.Payments.AntiCorruption.Interfaces;
using NerdStore.Payments.Business.Enums;
using NerdStore.Payments.Business.Interfaces;
using NerdStore.Payments.Business.Models;

namespace NerdStore.Payments.AntiCorruption.Implementations
{
    public class CreditCardPaymentFacade : ICreditCardPaymentFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IPayPalConfigurationManager _payPalConfigurationManager;

        public CreditCardPaymentFacade(IPayPalGateway payPalGateway, IPayPalConfigurationManager payPalConfigurationManager)
        {
            _payPalGateway = payPalGateway;
            _payPalConfigurationManager = payPalConfigurationManager;
        }

        public PaymentTransaction PayTheOrder(Order order, Payment payment)
        {
            bool isPaid = CommitPaypalTransaction(order, payment);
            var paymentTransactionStatus = isPaid ? PaymentTransactionStatus.Paid : PaymentTransactionStatus.Declined;

            return new PaymentTransaction(order.Id, payment.Id, payment.TotalPrice, paymentTransactionStatus);
        }

        private bool CommitPaypalTransaction(Order order, Payment payment)
        {
            var apiKey = _payPalConfigurationManager.GetApiKey();
            var encriptionKey = _payPalConfigurationManager.GetEncriptionKey();
            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, payment.CardNumber);

            bool isPaid = _payPalGateway.CommitTransaction(cardHashKey, order.Id.ToString(), payment.TotalPrice);

            return isPaid;
        }
    }
}
