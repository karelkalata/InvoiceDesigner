using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class CssStyleConfiguration : IEntityTypeConfiguration<CssStyle>
    {
        public void Configure(EntityTypeBuilder<CssStyle> cssStyle)
        {
            cssStyle.HasKey(e => e.Id);

            cssStyle.Property(e => e.Name)
                .IsRequired();

            cssStyle.Property(e => e.Value)
                .IsRequired();

            cssStyle.HasOne(e => e.DropItem)
                .WithMany(e => e.CssStyle)
                .HasForeignKey(e => e.DropItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
