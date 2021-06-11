using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Sales.Application.Commands.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events.Handlers
{
    public class PaidOrderEventHandler : INotificationHandler<PaidOrderEvent>
    {
        private readonly IMediatrHandler _mediatrHandler;

        public PaidOrderEventHandler(IMediatrHandler mediatrHandler)
        {
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(PaidOrderEvent message, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new FinishOrderCommand(message.CustomerId, message.OrderId));
        }
    }
 }
