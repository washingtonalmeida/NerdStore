using NerdStore.Core.Data;
using NerdStore.Sales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void Update(Order order);
        Task<Order> GetById(Guid id);
        Task<Order> GetDraftOrderByCustomerId(Guid customerId);
        Task<IEnumerable<Order>> GetByCustomerId(Guid customerId);

        void AddOrderItem(OrderItem orderItem);
        void UpdateOrderItem(OrderItem orderItem);
        void RemoveOrderItem(OrderItem orderItem);
        Task<OrderItem> GetOrderItemById(Guid orderItemId);
        Task<OrderItem> GetOrderItemByOrderIdAndProductId(Guid orderId, Guid productId);

        Task<Voucher> GetVoucherByCode(string code);
    }
}
