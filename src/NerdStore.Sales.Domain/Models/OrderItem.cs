using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Sales.Domain.Models
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int ProductQuantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        // Entity Framework Relationship
        public Order Order { get; set; }

        protected OrderItem() { }

        public OrderItem(Guid productId, string productName, int productQuantity, decimal unitPrice)
        {
            ProductId = productId;
            ProductName = productName;
            ProductQuantity = productQuantity;
            UnitPrice = unitPrice;
        }

        internal void AssociateToOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public decimal CalculatePrice()
        {
            return ProductQuantity * UnitPrice;
        }

        internal void AddToProductQuantity(int quantity)
        {
            ProductQuantity += quantity;
        }

        internal void UpdateProductQuantity(int quantity)
        {
            ProductQuantity = quantity;
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}
