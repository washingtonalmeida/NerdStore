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
    public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public UpdateOrderItemCommandHandler(IOrderRepository orderRepository, IMediatrHandler mediatrHandler)
        {
            _orderRepository = orderRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(UpdateOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await PublishErrorNotifications(message);
                return false;
            }

            if (!IsDraftOrderStarted(message.CustomerId))
            {
                await PublishOrderNotFoundEvent();
                return false;
            }

            var draftOrder = GetDraftOrderStarted(message.CustomerId);
            if (!draftOrder.HasOrderItemWithTheProduct(message.ProductId))
            {
                await PublishOrderItemNotFoundEvent();
                return false;
            }

            var orderItem = draftOrder.GetOrderItemByProductId(message.ProductId);
            draftOrder.UpdateProductQuantity(orderItem, message.ProductQuantity);
            draftOrder.AddEvent(new OrderUpdatedEvent(draftOrder.CustomerId, draftOrder.Id, draftOrder.TotalPrice));
            draftOrder.AddEvent(new OrderItemUpdatedEvent(draftOrder.CustomerId, draftOrder.Id, message.ProductId, message.ProductQuantity));

            PersistOrderItemUpdated(draftOrder, message.ProductId);
            PersistOrderUpdated(draftOrder);

            return await CommitChanges();
        }

        private async Task PublishErrorNotifications(UpdateOrderItemCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _mediatrHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        private bool IsDraftOrderStarted(Guid customerId)
        {
            return GetDraftOrderStarted(customerId) != null;
        }

        private async Task PublishOrderNotFoundEvent()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
        }

        private Order GetDraftOrderStarted(Guid customerId)
        {
            return _orderRepository.GetDraftOrderByCustomerId(customerId).Result;
        }

        private async Task PublishOrderItemNotFoundEvent()
        {
            await _mediatrHandler.PublishNotification(new DomainNotification("order", "Order item not found!"));
        }

        private void PersistOrderItemUpdated(Order order, Guid productId)
        {
            var orderItem = order.GetOrderItemByProductId(productId);
            _orderRepository.UpdateOrderItem(orderItem);
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
