using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Sales.Application.Commands.Models;
using NerdStore.Sales.Application.Events.Models;
using NerdStore.Sales.Domain.Interfaces;
using NerdStore.Sales.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands.Handlers
{
    public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public RemoveOrderItemCommandHandler(IOrderRepository orderRepository, IMediatrHandler mediatrHandler)
        {
            _orderRepository = orderRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(RemoveOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await PublishErrorNotifications(message);
                return false;
            }

            var draftOrder = GetDraftOrderStarted(message.CustomerId);

            if (draftOrder == null)
            {
                await PublishOrderNotFoundNotification();
                return false;
            }

            if (!draftOrder.HasOrderItemWithTheProduct(message.ProductId))
            {
                await PublishOrderItemNotFoundNotification();
                return false;
            }

            var orderItem = draftOrder.GetOrderItemByProductId(message.ProductId);
            draftOrder.RemoveOrderItem(orderItem);
            draftOrder.AddEvent(new OrderUpdatedEvent(draftOrder.CustomerId, draftOrder.Id, draftOrder.TotalPrice));
            draftOrder.AddEvent(new OrderItemRemovedEvent(draftOrder.CustomerId, draftOrder.Id, message.ProductId));

            PersistOrderItemRemoved(orderItem);
            PersistOrderUpdated(draftOrder);
            return await CommitChanges();
        }

        private async Task PublishErrorNotifications(RemoveOrderItemCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _mediatrHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        private Order GetDraftOrderStarted(Guid customerId)
        {
            return _orderRepository.GetDraftOrderByCustomerId(customerId).Result;
        }

        private async Task PublishOrderNotFoundNotification()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
        }

        private async Task PublishOrderItemNotFoundNotification()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("order", "Order item not found!"));
        }

        private void PersistOrderItemRemoved(OrderItem orderItem)
        {
            _orderRepository.RemoveOrderItem(orderItem);
        }

        private void PersistOrderUpdated(Order order)
        {
            _orderRepository.Update(order);
        }

        private async Task<bool> CommitChanges()
        {
            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
