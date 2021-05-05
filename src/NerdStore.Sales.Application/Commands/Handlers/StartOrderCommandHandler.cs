﻿using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Sales.Application.Commands.Models;
using NerdStore.Sales.Domain.Interfaces;
using NerdStore.Sales.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands.Handlers
{
    public class StartOrderCommandHandler : IRequestHandler<StartOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public StartOrderCommandHandler(IOrderRepository orderRepository, IMediatrHandler mediatrHandler)
        {
            _orderRepository = orderRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(StartOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await PublishCommandErrorsNotifications(message);
                return false;
            }

            var order = await _orderRepository.GetDraftOrderByCustomerId(message.CustomerId);
            if (order == null)
            {
                await PublishOrderNotFoundNotification();
                return false;
            }

            order.StartOrder();
            order.AddEvent(new OrderStartedEvent(message.CustomerId, message.OrderId, message.TotalPrice, message.CardName, message.CardNumber, 
                message.CardExpirationDate, message.CardCvv, GetOrderProductsDto(order)));

            PersistOrderStarted(order);

            return await CommitChanges();
        }

        private async Task PublishCommandErrorsNotifications(StartOrderCommand message)
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

        private OrderProductsDto GetOrderProductsDto(Order order)
        {
            var orderProductsDto = new OrderProductsDto();
            orderProductsDto.Products = new List<OrderProductDto>();
            foreach (var item in order.OrderItems)
            {
                orderProductsDto.Products.Add(new OrderProductDto(item.ProductId, item.ProductQuantity));
            }

            return orderProductsDto;
        }

        private void PersistOrderStarted(Order orderStarted)
        {
            _orderRepository.Update(orderStarted);
        }

        private async Task<bool> CommitChanges()
        {
            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
