using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Payments.Business.Models
{
    public class Order
    {
        public Guid Id { get; private set; }
        public decimal TotalPrice { get; private set; }
        public List<Product> Products { get; private set; }

        public Order(Guid id, decimal totalPrice)
        {
            Id = id;
            TotalPrice = totalPrice;
        }
    }
}
