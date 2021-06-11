using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Sales.Application.Commands.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events.Handlers
{
    public class OrderPaymentDeclinedEventHandler : INotificationHandler<OrderPaymentDeclinedEvent>
    {
        private readonly IMediatrHandler _mediatrHandler;

        public OrderPaymentDeclinedEventHandler(IMediatrHandler mediatrHandler)
        {
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(OrderPaymentDeclinedEvent message, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new CancelOrderProcessingAndSupplyStockCommand(message.CustomerId, message.OrderId));
        }
    }
}
