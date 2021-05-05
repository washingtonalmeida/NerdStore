using System;
using System.Collections.Generic;

namespace NerdStore.Core.DomainObjects.DTO
{
    public class OrderProductsDto
    {
        public Guid OrderId { get; set; }
        public ICollection<OrderProductDto> Products { get; set; }
    }
}
