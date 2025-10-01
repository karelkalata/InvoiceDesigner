using InvoiceDesigner.Domain.Shared.Models.Directories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> customer)
		{
			customer.HasKey(e => e.Id);

			customer.Property(e => e.Name)
				.IsRequired();

			customer.Property(e => e.TaxId)
				.HasMaxLength(50);

			customer.Property(e => e.VatId)
				.HasMaxLength(50);
		}
	}
}
