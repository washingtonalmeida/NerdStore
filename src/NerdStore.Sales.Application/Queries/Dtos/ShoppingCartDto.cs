
using System;
using System.Collections.Generic;

namespace NerdStore.Sales.Application.Queries.Dtos
{
    public class ShoppingCartDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Subtotal { get; set; }
        public string VoucherCode { get; set; }

        public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
        public ShoppingCartPaymentDto Payment { get; set; }
    }
}
