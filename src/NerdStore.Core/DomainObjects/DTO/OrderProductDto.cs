using System;

namespace NerdStore.Core.DomainObjects.DTO
{
    public class OrderProductDto
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }

        public OrderProductDto(Guid productId, int productQuantity)
        {
            ProductId = productId;
            ProductQuantity = productQuantity;
        }
    }
}
