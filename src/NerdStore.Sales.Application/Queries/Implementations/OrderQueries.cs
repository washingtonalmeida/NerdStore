using NerdStore.Sales.Application.Queries.Dtos;
using NerdStore.Sales.Application.Queries.Interfaces;
using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Interfaces;
using NerdStore.Sales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Queries.Implementations
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ShoppingCartDto> GetCustomerShoppingCart(Guid customerId)
        {
            var draftOrder = await _orderRepository.GetDraftOrderByCustomerId(customerId);
            if (draftOrder == null)
                return null;

            return MapOrderToShoppingCartDto(draftOrder);
        }

        private ShoppingCartDto MapOrderToShoppingCartDto(Order order)
        {
            var shoppingCart = new ShoppingCartDto
            {
                CustomerId = order.CustomerId,
                OrderId = order.Id,
                TotalPrice = order.TotalPrice,
                Discount = order.Discount,
                Subtotal = order.TotalPrice + order.Discount,
                VoucherCode = order?.Voucher?.Code,
                Items = MapOrderItemsToShoppingCartItemDtos(order.OrderItems).ToList()
            };
            return shoppingCart;
        }

        private IEnumerable<ShoppingCartItemDto> MapOrderItemsToShoppingCartItemDtos(IEnumerable<OrderItem> orderItems)
        {
            var shoppingCartItemDtos = new List<ShoppingCartItemDto>();

            if (orderItems == null)
                return shoppingCartItemDtos;

            foreach (var orderItem in orderItems)
            {
                shoppingCartItemDtos.Add(new ShoppingCartItemDto
                {
                    ProductId = orderItem.ProductId,
                    ProductName = orderItem.ProductName,
                    ProductQuantity = orderItem.ProductQuantity,
                    UnitPrice = orderItem.UnitPrice,
                    TotalPrice = orderItem.ProductQuantity * orderItem.UnitPrice
                });
            }

            return shoppingCartItemDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrders(Guid customerId)
        {
            var customerOrders = await _orderRepository.GetByCustomerId(customerId);
            customerOrders = KeepOnlyPaidOrCanceledOrders(customerOrders).OrderByDescending(x => x.Code).ToList();

            if (!customerOrders.Any())
                return null;

            return MapOrdersToOrderDtos(customerOrders);
        }

        private IEnumerable<Order> KeepOnlyPaidOrCanceledOrders(IEnumerable<Order> orders)
        {
            return orders.Where(x => x.OrderStatus == OrderStatus.Paid ||
                                     x.OrderStatus == OrderStatus.Canceled);
        }

        private IEnumerable<OrderDto> MapOrdersToOrderDtos(IEnumerable<Order> orders)
        {
            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                orderDtos.Add(new OrderDto
                {
                    Code = order.Code,
                    OrderStatus = (int) order.OrderStatus,
                    TotalPrice = order.TotalPrice,
                    RegistrationDate = order.RegistrationDate
                });
            }

            return orderDtos;
        }
    }
}
