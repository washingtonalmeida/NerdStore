using NerdStore.Core.DomainObjects;
using NerdStore.Sales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Sales.Domain.Models
{
    public class Order : Entity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems ;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        // Entity Framework Relationship
        public Voucher Voucher { get; private set; }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public Order(Guid customerId, bool voucherUsed, decimal discount, decimal totalPrice)
        {
            CustomerId = customerId;
            VoucherUsed = voucherUsed;
            Discount = discount;
            TotalPrice = totalPrice;
            _orderItems = new List<OrderItem>();
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            if (!orderItem.IsValid()) return;

            orderItem.AssociateToOrder(Id);

            if (HasOrderItemWithTheProduct(orderItem.ProductId))
            {
                UpdateProductQuantity(orderItem, orderItem.ProductQuantity);
            }
            else
            {
                _orderItems.Add(orderItem);
            }

            CalculateTotalPrice();
        }

        public bool HasOrderItemWithTheProduct(Guid productId)
        {
            return GetOrderItemByProductId(productId) != null;
        }

        public void UpdateProductQuantity(OrderItem orderItem, int quantity)
        {
            if(!IsItemInTheOrder(orderItem))
                throw new DomainException("The item does not belong to the order.");

            var orderItemToBeUpdated = GetOrderItemByProductId(orderItem.ProductId);
            orderItemToBeUpdated.UpdateProductQuantity(quantity);
            UpdateOrderItem(orderItemToBeUpdated);
        }

        public void UpdateOrderItem(OrderItem newOrderItem)
        {
            if (!IsItemInTheOrder(newOrderItem))
                throw new DomainException("The item does not belong to the order.");

            if (!newOrderItem.IsValid())
                throw new DomainException("Invalid order item.");

            var oldOrderItem = GetOrderItemByProductId(newOrderItem.ProductId);
            _orderItems.Remove(oldOrderItem);

            newOrderItem.AssociateToOrder(Id);
            _orderItems.Add(newOrderItem);

            CalculateTotalPrice();
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            if (!IsItemInTheOrder(orderItem))
                throw new DomainException("The item does not belong to the order.");

            var orderItemToBeRemoved = GetOrderItemByProductId(orderItem.ProductId);
            _orderItems.Remove(orderItemToBeRemoved);

            CalculateTotalPrice();
        }

        public void ApplyVoucher(Voucher voucher)
        {
            if (voucher.IsExpired())
                throw new DomainException("The voucher is expired. ");

            if (!voucher.HasQuantityAvailable())
                throw new DomainException("The voucher is not available.");

            if (voucher.IsUsed())
                throw new DomainException("The voucher has already been used.");

            if (!voucher.IsActive())
                throw new DomainException("The voucher is no longer valid.");

            Voucher = voucher;
            VoucherUsed = true;
            CalculateTotalPrice();
        }

        public void CalculateTotalPrice()
        {
            TotalPrice = OrderItems.Sum(orderItem => orderItem.CalculatePrice());
            CalculateDiscount();
        }

        private bool IsItemInTheOrder(OrderItem orderItem)
        {
            if (orderItem.OrderId != Id)
                return false;

            if (GetOrderItemByProductId(orderItem.ProductId) == null)
                return false;

            return true;
        }

        public OrderItem GetOrderItemByProductId(Guid productId)
        {
            return _orderItems.FirstOrDefault(p => p.ProductId == productId);
        }

        public void CalculateDiscount()
        {
            if (!VoucherUsed) return;

            Discount = Voucher.VoucherDiscountType == VoucherDiscountType.Percentage ? GetDiscountByPercentage() : GetDiscountByMonetaryValue();
            TotalPrice -= Discount;
            TotalPrice = TotalPrice > 0 ? TotalPrice : 0;
        }

        private decimal GetDiscountByPercentage()
        {
            if(Voucher.Percentage == null)
                throw new DomainException("The voucher is invalid.");

            return (TotalPrice * Voucher.Percentage.Value) / 100;
        }

        private decimal GetDiscountByMonetaryValue()
        {
            if (Voucher.MonetaryValue == null)
                throw new DomainException("The voucher is invalid.");

            return Voucher.MonetaryValue.Value;
        }


        public void DraftOrder()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public void StartOrder()
        {
            OrderStatus = OrderStatus.Started;
        }

        public void FinishOrder()
        {
            OrderStatus = OrderStatus.Paid;
        }

        public void CancelOrder()
        {
            OrderStatus = OrderStatus.Canceled;
        }

        public static class OrderFactory
        {
            public static Order CreateDraftOrder(Guid customerId)
            {
                var order = new Order { CustomerId = customerId };
                order.DraftOrder();
                return order;
            }
        }
    }
}
