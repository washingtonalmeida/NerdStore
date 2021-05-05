
namespace NerdStore.Sales.Application.Queries.Dtos
{
    public class ShoppingCartPaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }
    }
}
