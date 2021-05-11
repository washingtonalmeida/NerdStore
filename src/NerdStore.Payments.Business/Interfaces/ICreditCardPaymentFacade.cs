using NerdStore.Payments.Business.Models;

namespace NerdStore.Payments.Business.Interfaces
{
    public interface ICreditCardPaymentFacade
    {
        PaymentTransaction PayTheOrder(Order order, Payment payment);
    }
}
