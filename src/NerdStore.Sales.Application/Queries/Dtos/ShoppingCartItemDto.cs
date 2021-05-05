
using System;

namespace NerdStore.Sales.Application.Queries.Dtos
{
    public class ShoppingCartItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
