
using NerdStore.Catalog.Domain.Events;
using NerdStore.Catalog.Domain.Interfaces;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using System;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public StockService(IProductRepository productRepository,
                            IMediatrHandler mediatrHandler)
        {
            _productRepository = productRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> DebitStock(OrderProductsDto orderProductsDto)
        {
            foreach (var product in orderProductsDto.Products)
            {
                if(!await DebitStock(product.ProductId, product.ProductQuantity))
                    return false;
            }

            return true;
        }

        public async Task<bool> DebitStock(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) return false;

            if (!product.HasStock(quantity))
            {
                await _mediatrHandler.PublishNotification(new DomainNotification("Stock", $"The product {product.Name} is out of stock."));
                return false;
            }
            product.DebitStock(quantity);

            // TODO: To parameterize the low stock quantity 
            if (product.StockQuantity < 10)
            {
                await _mediatrHandler.PublishEvent(new LowProductStockEvent(product.Id, product.StockQuantity));
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

            return true;
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
