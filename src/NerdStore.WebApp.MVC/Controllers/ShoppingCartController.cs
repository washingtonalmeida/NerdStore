using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Application.Interfaces;
using NerdStore.Core.Communication.Mediatr;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Sales.Application.Commands.Models;
using NerdStore.Sales.Application.Queries.Dtos;
using NerdStore.Sales.Application.Queries.Interfaces;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class ShoppingCartController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IOrderQueries _orderQueries;
        private readonly IMediatrHandler _mediatrHandler;

        public ShoppingCartController(INotificationHandler<DomainNotification> notifications,
                                      IProductAppService productAppService,
                                      IOrderQueries orderQueries,
                                      IMediatrHandler mediatrHandler) : base(notifications, mediatrHandler)
        {
            _productAppService = productAppService;
            _orderQueries = orderQueries;
            _mediatrHandler = mediatrHandler;
        }

        [HttpGet]
        [Route("my-shopping-cart")]
        public async Task<IActionResult> Index()
        {
            var shoppingCart = await _orderQueries.GetCustomerShoppingCart(CustomerId);
            return View(shoppingCart);
        }

        [HttpPost]
        [Route("my-shopping-cart")]
        public async Task<IActionResult> AddOrderItem(Guid id, int productQuantity)
        {
            var product = await _productAppService.GetById(id);

            if (product == null) return BadRequest();

            if (product.StockQuantity < productQuantity)
            {
                TempData["Error"] = "Product with insufficient stock.";
                return RedirectToAction("ProductDetails", "ShopWindow", new { id });
            }

            var command = new AddOrderItemCommand(CustomerId, product.Id, product.Name, productQuantity, product.Price);
            await _mediatrHandler.SendCommand(command);

            if (IsOperationValid()) 
            {
                return RedirectToAction("Index");
            }

            TempData["Errors"] = GetErrorMessages();
            return RedirectToAction("ProductDetails", "ShopWindow", new { id });
        }

        [HttpPost]
        [Route("remove-order-item")]
        public async Task<IActionResult> RemoveOrderItem(Guid orderId, Guid productId)
        {
            var command = new RemoveOrderItemCommand(CustomerId, orderId, productId);
            await _mediatrHandler.SendCommand(command);

            if (IsOperationValid())
                return RedirectToAction("Index");

            var shoppingCart = await _orderQueries.GetCustomerShoppingCart(CustomerId);

            return View("Index", shoppingCart);
        }


        [HttpPost]
        [Route("update-order-item")]
        public async Task<IActionResult> UpdateOrderItem(Guid orderId, Guid productId, int productQuantity)
        {
            var command = new UpdateOrderItemCommand(CustomerId, orderId, productId, productQuantity);
            await _mediatrHandler.SendCommand(command);

            if (IsOperationValid())
                return RedirectToAction("Index");

            var shoppingCart = await _orderQueries.GetCustomerShoppingCart(CustomerId);

            return View("Index", shoppingCart);
        }

        [HttpPost]
        [Route("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(Guid orderId, string voucherCode)
        {
            var command = new ApplyVoucherToOrderCommand(CustomerId, orderId, voucherCode);
            await _mediatrHandler.SendCommand(command);

            if (IsOperationValid())
                return RedirectToAction("Index");

            var shoppingCart = await _orderQueries.GetCustomerShoppingCart(CustomerId);

            return View("Index", shoppingCart);
        }

        [HttpGet]
        [Route("purchase-summary")]
        public async Task<IActionResult> PurchaseSummary()
        {
            var shoppingCart = await _orderQueries.GetCustomerShoppingCart(CustomerId);
            return View(shoppingCart);
        }

        [HttpPost]
        [Route("start-order")]
        public async Task<IActionResult> StartOrder(ShoppingCartDto shoppingCartDto)
        {
            var command = new StartOrderCommand(CustomerId, shoppingCartDto.OrderId, shoppingCartDto.TotalPrice, shoppingCartDto.Payment.CardName, shoppingCartDto.Payment.CardNumber, 
                shoppingCartDto.Payment.CardExpirationDate, shoppingCartDto.Payment.CardCvv);

            await _mediatrHandler.SendCommand(command);

            if (IsOperationValid())
            {
                return RedirectToAction("Index", "Order");
            }

            shoppingCartDto = await _orderQueries.GetCustomerShoppingCart(CustomerId);
            return View("PurchaseSummary", shoppingCartDto);
        }
    }
}