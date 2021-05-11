using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Payments.Business.Interfaces;
using NerdStore.Payments.Business.Models;
using System.Threading.Tasks;

namespace NerdStore.Payments.Business.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ICreditCardPaymentFacade _creditCardPaymentFacade;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public PaymentService(ICreditCardPaymentFacade creditCardPaymentFacade, IPaymentRepository paymentRepository, IMediatrHandler mediatrHandler)
        {
            _creditCardPaymentFacade = creditCardPaymentFacade;
            _paymentRepository = paymentRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<PaymentTransaction> PayTheOrder(OrderPaymentDto orderPaymentDto)
        {
            var order = CreateOrder(orderPaymentDto);
            var payment = CreatePayment(orderPaymentDto);

            var paymentTransaction = PayTheOrder(order, payment);
            if (paymentTransaction.IsPaid())
            {
                PersistPaymentAdded(payment);
                PersistPaymentTransactionAdded(paymentTransaction);
                await CommitChanges();

                payment.AddEvent(new PaidOrderEvent(orderPaymentDto.CustomerId, orderPaymentDto.OrderId, payment.Id, paymentTransaction.Id, orderPaymentDto.TotalPrice));
                await PublishPaymentEvents(payment);
            }
            else
            {
                await PublishTransactionDeclinedNotification();
                await PublichOrderPaymentDeclinedEvent(orderPaymentDto, payment, paymentTransaction);
            }

            return paymentTransaction;
        }

        private Order CreateOrder(OrderPaymentDto orderPaymentDto)
        {
            return new Order(orderPaymentDto.OrderId, orderPaymentDto.TotalPrice);
        }

        private Payment CreatePayment(OrderPaymentDto orderPaymentDto)
        {
            return new Payment(orderPaymentDto.OrderId, orderPaymentDto.TotalPrice, orderPaymentDto.CardName, orderPaymentDto.CardName, orderPaymentDto.CardExpirationDate, orderPaymentDto.CardCvv);
        }

        private PaymentTransaction PayTheOrder(Order order, Payment payment)
        {
            return _creditCardPaymentFacade.PayTheOrder(order, payment);
        }

        private void PersistPaymentAdded(Payment payment)
        {
            _paymentRepository.Add(payment);
        }

        private void PersistPaymentTransactionAdded(PaymentTransaction paymentTransaction)
        {
            _paymentRepository.AddPaymentTransaction(paymentTransaction);
        }

        private async Task CommitChanges()
        {
            await _paymentRepository.UnitOfWork.Commit();
        }

        private async Task PublishPaymentEvents(Payment payment)
        {
            if (!payment.HasNotifications())
                return;

            foreach (var paymentEvent in payment.Notifications)
            {
                await _mediatrHandler.PublishEvent(paymentEvent);
            }
        }

        private async Task PublishTransactionDeclinedNotification()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("payment", "The credit card company declined the payment."));
        }

        private async Task PublichOrderPaymentDeclinedEvent(OrderPaymentDto orderPaymentDto, Payment payment, PaymentTransaction paymentTransaction)
        {
            await _mediatrHandler.PublishEvent(new OrderPaymentDeclinedEvent(orderPaymentDto.CustomerId, orderPaymentDto.OrderId, payment.Id, paymentTransaction.Id, orderPaymentDto.TotalPrice));
        }
    }
}
