
using NerdStore.Catalog.Domain.Events;
using NerdStore.Catalog.Domain.Interfaces;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.DomainObjects.DTO;
using System;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatrHandler _bus;

        public StockService(IProductRepository productRepository,
                            IMediatrHandler bus)
        {
            _productRepository = productRepository;
            _bus = bus;
        }

        public async Task<bool> DebitStock(OrderProductsDto orderProductsDto)
        {
            foreach (var product in orderProductsDto.Products)
            {
                if(!await DebitStock(product.ProductId, product.ProductQuantity))
                    return false;
            }

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitStock(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) return false;
     
            if (!product.HasStock(quantity)) return false;

            product.DebitStock(quantity);

            // TODO: To parameterize the low stock quantity 
            if (product.StockQuantity < 10)
            {
                await _bus.PublishEvent(new LowProductStockEvent(product.Id, product.StockQuantity));
            }

            _productRepository.Update(product);
            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> SupplyStock(OrderProductsDto orderProductsDto)
        {
            foreach (var product in orderProductsDto.Products)
            {
                if (!await SupplyStock(product.ProductId, product.ProductQuantity))
                    return false;
            }

            return await _productRepository.UnitOfWork.Commit();
        }

        public async Task<bool> SupplyStock(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) { return false; }

            product.SupplyStock(quantity);

            _productRepository.Update(product);
            return await _productRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _productRepository.Dispose();
        }
    }
}
