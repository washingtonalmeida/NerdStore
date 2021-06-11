using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Sales.Application.Commands.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events.Handlers
{
    public class InsufficientStockForOrderEventHandler : INotificationHandler<InsufficientStockForOrderEvent>
    {
        private readonly IMediatrHandler _mediatrHandler;

        public InsufficientStockForOrderEventHandler(IMediatrHandler mediatrHandler)
        {
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(InsufficientStockForOrderEvent message, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new CancelOrderProcessingCommand(message.CustomerId, message.OrderId));
        }
    }
}
