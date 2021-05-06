using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Payments.Business.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Product> Products { get; set; }
    }
}
