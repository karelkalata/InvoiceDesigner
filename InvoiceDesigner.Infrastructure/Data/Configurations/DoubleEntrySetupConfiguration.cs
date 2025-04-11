using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class DoubleEntrySetupConfiguration : IEntityTypeConfiguration<DoubleEntrySetup>
    {
        public void Configure(EntityTypeBuilder<DoubleEntrySetup> doubleEntrySetup)
        {
            doubleEntrySetup.HasKey(e => e.Id);

            doubleEntrySetup.HasOne(e => e.CreditAccount)
                .WithMany()
                .HasForeignKey(e => e.Credit)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            doubleEntrySetup.HasOne(e => e.DebitAccount)
                .WithMany()
                .HasForeignKey(e => e.Debit)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            doubleEntrySetup.Property(e => e.EntryMode).IsRequired();
            doubleEntrySetup.Property(e => e.AmountType).IsRequired();
        }
    }
}
