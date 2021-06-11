﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Catalog.Domain.Models;

namespace NerdStore.Catalog.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Description)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(c => c.Image)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Price)
                .IsRequired()
                .HasColumnType("DECIMAL(10,2)");

            builder.OwnsOne(c => c.Dimensions, cm =>
            {
                cm.Property(c => c.Height)
                    .HasColumnName("Height")
                    .HasColumnType("int");

                cm.Property(c => c.Width)
                    .HasColumnName("Width")
                    .HasColumnType("int");

                cm.Property(c => c.Depth)
                    .HasColumnName("Depth")
                    .HasColumnType("int");
            });

            builder.ToTable("Product");
        }
    }
}
