﻿using MediatR;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Sales.Application.Events.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events.Handlers
{
    public class OrderEventHandler :
        INotificationHandler<DraftOrderStartedEvent>,
        INotificationHandler<OrderUpdatedEvent>,
        INotificationHandler<OrderItemAddedEvent>,
        INotificationHandler<InsufficientStockForOrderEvent>
    {
        public Task Handle(DraftOrderStartedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(OrderItemAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(InsufficientStockForOrderEvent notification, CancellationToken cancellationToken)
        {
            //TODO: Cancel order processing 
            throw new System.NotImplementedException();
        }
    }
}
