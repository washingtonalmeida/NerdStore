using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Sales.Application.Commands.Models;
using NerdStore.Sales.Application.Events.Models;
using NerdStore.Sales.Domain.Interfaces;
using NerdStore.Sales.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands.Handlers
{
    public class FinishOrderCommandHandler : IRequestHandler<FinishOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public FinishOrderCommandHandler(IOrderRepository orderRepository, IMediatrHandler mediatrHandler)
        {
            _orderRepository = orderRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(FinishOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await PublishInvalidCommandNotifications(message);
                return false;
            }

            var order = await _orderRepository.GetById(message.OrderId);
            if (order == null)
            {
                await PublishOrderNotFoundNotification();
            }

            order.FinishOrder();
            order.AddEvent(new OrderFinishedEvent(order.Id));

            PersistFinishedOrder(order);

            return await CommitChanges();
        }

        private async Task PublishInvalidCommandNotifications(FinishOrderCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _mediatrHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        private async Task PublishOrderNotFoundNotification()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
        }

        private void PersistFinishedOrder(Order order)
        {
            _orderRepository.Update(order);
        }

        private async Task<bool> CommitChanges()
        {
            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
