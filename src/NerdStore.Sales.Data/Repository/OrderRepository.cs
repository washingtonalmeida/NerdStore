
using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Interfaces;
using NerdStore.Sales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Sales.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SalesContext _context;

        public OrderRepository(SalesContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<Order> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order> GetDraftOrderByCustomerId(Guid customerId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.CustomerId == customerId && o.OrderStatus == OrderStatus.Draft);
            if (order == null)
                return null;

            await _context.Entry(order).Collection(x => x.OrderItems).LoadAsync();

            if (order.VoucherId != null)
            {
                await _context.Entry(order).Reference(x => x.Voucher).LoadAsync();
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetByCustomerId(Guid customerId)
        {
            return await _context.Orders.AsNoTracking().Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
        }

        public async Task<OrderItem> GetOrderItemById(Guid orderItemId)
        {
            return await _context.OrderItems.FindAsync(orderItemId);
        }

        public async Task<OrderItem> GetOrderItemByOrderIdAndProductId(Guid orderId, Guid productId)
        {
            return await _context.OrderItems.FirstOrDefaultAsync(x => x.OrderId == orderId && x.ProductId == productId);
        }

        public async Task<Voucher> GetVoucherByCode(string code)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(x => x.Code == code);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
