using InvoiceDesigner.Domain.Shared.Models.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
	public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
	{
		public void Configure(EntityTypeBuilder<Invoice> invoice)
		{
			invoice.HasKey(e => e.Id);

			invoice.HasOne(e => e.Company)
				.WithMany()
				.HasForeignKey(e => e.CompanyId)
				.IsRequired();

			invoice.Property(e => e.Vat)
				.HasDefaultValue(decimal.Zero);

			invoice.Property(e => e.EnabledVat)
				.HasDefaultValue(true);

			invoice.HasOne(e => e.Customer)
				.WithMany()
				.HasForeignKey(e => e.CustomerId)
				.IsRequired();

			invoice.HasOne(e => e.Currency)
				.WithMany()
				.HasForeignKey(e => e.CurrencyId)
				.IsRequired();

			invoice.HasOne(e => e.Bank)
				.WithMany()
				.HasForeignKey(e => e.BankId)
				.IsRequired();

			invoice.Property(e => e.Amount)
				.HasDefaultValue(decimal.Zero);

			invoice.HasMany(e => e.InvoiceItems)
				.WithOne()
				.IsRequired();
		}
	}
}
