using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Payments.Business.Models;
using System.Threading.Tasks;

namespace NerdStore.Payments.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentTransaction> PayTheOrder(OrderPaymentDto orderPayment);
    }
}
