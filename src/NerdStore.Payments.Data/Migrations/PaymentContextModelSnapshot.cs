﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NerdStore.Payments.Data;

namespace NerdStore.Payments.Data.Migrations
{
    [DbContext(typeof(PaymentContext))]
    partial class PaymentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NerdStore.Payments.Business.Models.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CardCvv")
                        .IsRequired()
                        .HasColumnType("varchar(4)");

                    b.Property<string>("CardExpirationDate")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("CardName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("varchar(16)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("NerdStore.Payments.Business.Models.PaymentTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("PaymentTransaction");
                });

            modelBuilder.Entity("NerdStore.Payments.Business.Models.PaymentTransaction", b =>
                {
                    b.HasOne("NerdStore.Payments.Business.Models.Payment", "Payment")
                        .WithOne("PaymentTransaction")
                        .HasForeignKey("NerdStore.Payments.Business.Models.PaymentTransaction", "PaymentId")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
