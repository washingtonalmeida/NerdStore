using Microsoft.AspNetCore.Mvc;
using NerdStore.Sales.Application.Queries.Interfaces;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Extensions
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IOrderQueries _orderQueries;

        // TODO: Get logged customer
        protected Guid CustomerId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32");

        public ShoppingCartViewComponent(IOrderQueries orderQueries)
        {
            _orderQueries = orderQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var shoppingCart = await _orderQueries.GetCustomerShoppingCart(CustomerId);
            var itemsQuantity = shoppingCart?.Items?.Count ?? 0;

            return View(itemsQuantity);
        }
    }
}
