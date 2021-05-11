using NerdStore.Core.Data;
using NerdStore.Payments.Business.Interfaces;
using NerdStore.Payments.Business.Models;

namespace NerdStore.Payments.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            _context.PaymentTransactions.Add(paymentTransaction);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
