
using NerdStore.Core.DomainObjects.DTO;
using System;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Domain.Interfaces
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStock(OrderProductsDto orderProductsDto);
        Task<bool> DebitStock(Guid productId, int quantity);
        Task<bool> SupplyStock(OrderProductsDto orderProductsDto);
        Task<bool> SupplyStock(Guid productId, int quantity);
    }
}
