using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Payments.Business.Models;

namespace NerdStore.Payments.Data.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CardName)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(x => x.CardNumber)
                .IsRequired()
                .HasColumnType("varchar(16)");

            builder.Property(x => x.CardExpirationDate)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(x => x.CardCvv)
                .IsRequired()
                .HasColumnType("varchar(4)");

            // 1: 1 => Payment : PaymentTransaction
            builder.HasOne(x => x.PaymentTransaction)
                .WithOne(x => x.Payment);

            builder.ToTable("Payment");
        }
    }
}
