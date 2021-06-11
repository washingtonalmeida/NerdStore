using MediatR;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Sales.Application.Commands.Models;
using NerdStore.Sales.Domain.Interfaces;
using NerdStore.Sales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands.Handlers
{
    public class CancelOrderProcessingAndSupplyStockCommandHandler : IRequestHandler<CancelOrderProcessingAndSupplyStockCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public CancelOrderProcessingAndSupplyStockCommandHandler(IOrderRepository orderRepository, IMediatrHandler mediatrHandler)
        {
            _orderRepository = orderRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(CancelOrderProcessingAndSupplyStockCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await PublishInvalidCommandNotifications(message);
                return false;
            }

            var order = await GetOrderById(message.OrderId);
            if (order == null)
            {
                await PublishOrderNotFoundNotification();
                return false;
            }

            order.DraftOrder();
            order.AddEvent(new OrderProcessingCanceledEvent(order.CustomerId, order.Id, GetOrderProductsDto(order)));

            PersistOrder(order);

            return await CommitChanges();
        }

        private async Task PublishInvalidCommandNotifications(CancelOrderProcessingAndSupplyStockCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                await _mediatrHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        private async Task<Order> GetOrderById(Guid orderId)
        {
            return await _orderRepository.GetById(orderId);
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

        private void PersistOrder(Order order)
        {
            _orderRepository.Update(order);
        }

        private async Task<bool> CommitChanges()
        {
            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
