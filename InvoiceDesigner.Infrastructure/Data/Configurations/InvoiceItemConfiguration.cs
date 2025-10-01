using InvoiceDesigner.Domain.Shared.Models.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
	public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
	{
		public void Configure(EntityTypeBuilder<InvoiceItem> invoiceItem)
		{
			invoiceItem.HasKey(e => e.Id);

			invoiceItem.HasOne(e => e.Invoice)
				.WithMany(e => e.InvoiceItems)
				.HasForeignKey(e => e.InvoiceId)
				.IsRequired();

			invoiceItem.HasOne(e => e.Item)
				.WithMany()
				.HasForeignKey(e => e.ItemId)
				.IsRequired();

			invoiceItem.Property(e => e.Price)
				.IsRequired()
				.HasDefaultValue(decimal.Zero);

			invoiceItem.Property(e => e.Quantity)
				.IsRequired()
				.HasDefaultValue(decimal.Zero);
		}
	}
}
