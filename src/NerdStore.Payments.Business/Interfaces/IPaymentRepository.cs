using NerdStore.Core.Data;
using NerdStore.Payments.Business.Models;

namespace NerdStore.Payments.Business.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        void Add(Payment payment);
        void AddPaymentTransaction(PaymentTransaction paymentTransaction);
    }
}
