using System;

namespace NerdStore.Sales.Application.Queries.Dtos
{
    public class OrderDto
    {
        public int Code { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int OrderStatus { get; set; }
    }
}
