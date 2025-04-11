using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class DropItemConfiguration : IEntityTypeConfiguration<DropItem>
    {
        public void Configure(EntityTypeBuilder<DropItem> dropItem)
        {
            dropItem.HasKey(e => e.Id);

            dropItem.Property(e => e.UniqueId)
                .IsRequired();

            dropItem.Property(e => e.Selector)
                .IsRequired();

            dropItem.Property(e => e.StartSelector)
                .IsRequired();

            dropItem.HasMany(e => e.CssStyle)
                .WithOne(e => e.DropItem)
                .HasForeignKey(e => e.DropItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
