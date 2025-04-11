using System;

using InvoiceDesigner.Domain.Shared.Models.Directories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> currency)
        {
            currency.HasKey(e => e.Id);

            currency.HasIndex(e => e.Name).IsUnique();

            currency.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(3);

            currency.Property(e => e.Description)
                .HasMaxLength(100);

            currency.HasData(
                new Currency { Id = 1, Name = "USD", Description = "US Dollar" },
                new Currency { Id = 2, Name = "EUR", Description = "Euro" },
                new Currency { Id = 3, Name = "CZK", Description = "Czech Koruna" }
            );
        }
    }
}

