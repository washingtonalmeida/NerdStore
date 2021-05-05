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
    public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public AddOrderItemCommandHandler(IOrderRepository orderRepository,
                                   IMediatrHandler mediatrHandler)
        {
            _orderRepository = orderRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await PublishErrorNotifications(message);
                return false;
            }

            if (IsDraftOrderStarted(message.CustomerId))
            {
                UpdateDraftOrder(message);
            }
            else
            {
                AddDraftOrder(message);
            }

            return await CommitChanges();
        }

        private async Task PublishErrorNotifications(AddOrderItemCommand message)
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

        private void UpdateDraftOrder(AddOrderItemCommand message)
        {
            var draftOrder = GetDraftOrderStarted(message.CustomerId);
            var isItemAlreadyInOrder = draftOrder.HasOrderItemWithTheProduct(message.ProductId);
            draftOrder.AddOrderItem(CreateOrderItem(message));

            if (isItemAlreadyInOrder)
            {
                PersistOrderItemUpdated(draftOrder, message.ProductId);
            }
            else
            {
                PersistOrderItemAdded(draftOrder, message.ProductId);
            }

            draftOrder.AddEvent(new OrderUpdatedEvent(draftOrder.CustomerId, draftOrder.Id, draftOrder.TotalPrice));
            draftOrder.AddEvent(new OrderItemAddedEvent(draftOrder.CustomerId, draftOrder.Id, message.ProductId, message.ProductName,
                                                        message.UnitPrice, message.ProductQuantity));
        }

        private void AddDraftOrder(AddOrderItemCommand message)
        {
            var draftOrder = CreateDraftOrder(message);

            _orderRepository.Add(draftOrder);

            draftOrder.AddEvent(new DraftOrderStartedEvent(message.CustomerId, message.ProductId));
            draftOrder.AddEvent(new OrderItemAddedEvent(draftOrder.CustomerId, draftOrder.Id, message.ProductId, message.ProductName,
                                                        message.UnitPrice, message.ProductQuantity));
        }


        private Order GetDraftOrderStarted(Guid customerId)
        {
            return _orderRepository.GetDraftOrderByCustomerId(customerId).Result;
        }

        private Order CreateDraftOrder(AddOrderItemCommand message)
        {
            var draftOrder = Order.OrderFactory.CreateDraftOrder(message.CustomerId);
            draftOrder.AddOrderItem(CreateOrderItem(message));
            return draftOrder;
        }

        private OrderItem CreateOrderItem(AddOrderItemCommand message)
        {
            return new OrderItem(message.ProductId, message.ProductName, message.ProductQuantity, message.UnitPrice);
        }

        private void PersistOrderItemUpdated(Order draftOrder, Guid productId)
        {
            _orderRepository.UpdateOrderItem(draftOrder.GetOrderItemByProductId(productId));
        }

        private void PersistOrderItemAdded(Order draftOrder, Guid productId)
        {
            _orderRepository.AddOrderItem(draftOrder.GetOrderItemByProductId(productId));
        }

        private async Task<bool> CommitChanges()
        {
            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
