using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
	public class FormDesignerConfiguration : IEntityTypeConfiguration<FormDesigner>
	{
		public void Configure(EntityTypeBuilder<FormDesigner> formDesigner)
		{
			formDesigner.HasKey(e => e.Id);

			formDesigner.Property(e => e.Name)
				.IsRequired();

			formDesigner.HasMany(e => e.Schemes)
				.WithOne()
				.HasForeignKey(e => e.FormDesignerId)
				.OnDelete(DeleteBehavior.Cascade);

			formDesigner.HasMany(e => e.DropItems)
				.WithOne()
				.HasForeignKey(e => e.FormDesignerSchemeId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
