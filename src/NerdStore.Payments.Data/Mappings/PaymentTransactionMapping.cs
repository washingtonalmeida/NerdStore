using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Payments.Business.Models;

namespace NerdStore.Payments.Data.Mappings
{
    public class PaymentTransactionMapping : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("PaymentTransaction");
        }
    }
}
