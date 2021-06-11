using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalog.Application.Interfaces;
using NerdStore.Catalog.Application.Services;
using NerdStore.Catalog.Data;
using NerdStore.Catalog.Data.Repository;
using NerdStore.Catalog.Domain.Events;
using NerdStore.Catalog.Domain.Interfaces;
using NerdStore.Catalog.Domain.Services;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Payments.AntiCorruption.Implementations;
using NerdStore.Payments.AntiCorruption.Interfaces;
using NerdStore.Payments.Business.Events;
using NerdStore.Payments.Business.Implementations;
using NerdStore.Payments.Business.Interfaces;
using NerdStore.Payments.Data;
using NerdStore.Payments.Data.Repository;
using NerdStore.Sales.Application.Commands.Handlers;
using NerdStore.Sales.Application.Commands.Models;
using NerdStore.Sales.Application.Events.Handlers;
using NerdStore.Sales.Application.Events.Models;
using NerdStore.Sales.Application.Queries.Implementations;
using NerdStore.Sales.Application.Queries.Interfaces;
using NerdStore.Sales.Data;
using NerdStore.Sales.Data.Repository;
using NerdStore.Sales.Domain.Interfaces;

namespace NerdStore.WebApp.MVC.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Mediatr
            services.AddScoped<IMediatrHandler, MediatrHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Catalog
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CatalogContext>();

            services.AddScoped<INotificationHandler<LowProductStockEvent>, ProductEventHandler>();
            services.AddScoped<INotificationHandler<OrderStartedEvent>, ProductEventHandler>();
            services.AddScoped<INotificationHandler<OrderProcessingCanceledEvent>, ProductEventHandler>();

            // Sales
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderQueries, OrderQueries>();
            services.AddScoped<SalesContext>();

            services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, AddOrderItemCommandHandler>();
            services.AddScoped<IRequestHandler<ApplyVoucherToOrderCommand, bool>, ApplyVoucherToOrderCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveOrderItemCommand, bool>, RemoveOrderItemCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateOrderItemCommand, bool>, UpdateOrderItemCommandHandler>();
            services.AddScoped<IRequestHandler<StartOrderCommand, bool>, StartOrderCommandHandler>();
            services.AddScoped<IRequestHandler<FinishOrderCommand, bool>, FinishOrderCommandHandler>();
            services.AddScoped<IRequestHandler<CancelOrderProcessingCommand, bool>, CancelOrderProcessingCommandHandler>();
            services.AddScoped<IRequestHandler<CancelOrderProcessingAndSupplyStockCommand, bool>, CancelOrderProcessingAndSupplyStockCommandHandler>();

            services.AddScoped<INotificationHandler<DraftOrderStartedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderUpdatedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderItemAddedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<InsufficientStockForOrderEvent>, InsufficientStockForOrderEventHandler>();
            services.AddScoped<INotificationHandler<PaidOrderEvent>, PaidOrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderPaymentDeclinedEvent>, OrderPaymentDeclinedEventHandler>();

            // Payment
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<PaymentContext>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICreditCardPaymentFacade, CreditCardPaymentFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();
            services.AddScoped<IPayPalConfigurationManager, PayPalConfigurationManager>();

            services.AddScoped<INotificationHandler<StockReservedForOrderEvent>, StockReservedForOrderEventHandler>();
        }
    }
}
