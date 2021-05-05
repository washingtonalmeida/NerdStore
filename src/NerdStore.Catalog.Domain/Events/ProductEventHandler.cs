
using MediatR;
using NerdStore.Catalog.Domain.Interfaces;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Domain.Events
{
    public class ProductEventHandler : 
        INotificationHandler<LowProductStockEvent>,
        INotificationHandler<OrderStartedEvent>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMediatrHandler _mediatrHandler;

        public ProductEventHandler(IProductRepository productRepository,
                                   IStockService stockService,
                                   IMediatrHandler mediatrHandler)
        {
            _productRepository = productRepository;
            _stockService = stockService;
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(LowProductStockEvent message, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(message.AggregateId);

            // TODO: Send an email to purchase more products for stock
        }

        public async Task Handle(OrderStartedEvent message, CancellationToken cancellationToken)
        {
            bool IsStockReservedForOrder = await _stockService.DebitStock(message.OrderProducts);
            if (IsStockReservedForOrder)
            {
                await _mediatrHandler.PublishEvent(new StockReservedForOrderEvent(message.CustomerId, message.OrderId, message.TotalPrice, message.CardName, message.CardNumber, 
                    message.CardExpirationDate, message.CardCvv, message.OrderProducts));
            }
            else
            {
                await _mediatrHandler.PublishEvent(new InsufficientStockForOrderEvent(message.CustomerId, message.OrderId));
            }
        }
    }
}
