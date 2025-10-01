using InvoiceDesigner.Domain.Shared.Models.Directories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> product)
		{
			product.HasKey(e => e.Id);

			product.Property(e => e.Name)
				.IsRequired()
				.HasMaxLength(200);

			product.HasMany(e => e.ProductPrice)
				.WithOne()
				.HasForeignKey(e => e.ProductId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
