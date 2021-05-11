using MediatR;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Payments.Business.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Payments.Business.Events
{
    public class StockReservedForOrderEventHandler : INotificationHandler<StockReservedForOrderEvent>
    {
        private readonly IPaymentService _paymentService;

        public StockReservedForOrderEventHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Handle(StockReservedForOrderEvent message, CancellationToken cancellationToken)
        {
            var orderPaymentDto = CreateOrderPaymentDto(message);
            await _paymentService.PayTheOrder(orderPaymentDto);
        }

        private OrderPaymentDto CreateOrderPaymentDto(StockReservedForOrderEvent message)
        {
            return new OrderPaymentDto
            {
                CustomerId = message.CustomerId,
                OrderId = message.OrderId,
                TotalPrice = message.TotalPrice,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                CardExpirationDate = message.CardExpirationDate,
                CardCvv = message.CardCvv
            };
        }
    }
}
