using NerdStore.Sales.Application.Queries.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Queries.Interfaces
{
    public interface IOrderQueries
    {
        Task<ShoppingCartDto> GetCustomerShoppingCart(Guid customerId);
        Task<IEnumerable<OrderDto>> GetCustomerOrders(Guid customerId);
    }
}
