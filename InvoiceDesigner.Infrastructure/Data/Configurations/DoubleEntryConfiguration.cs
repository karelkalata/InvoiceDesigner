using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class DoubleEntryConfiguration : IEntityTypeConfiguration<DoubleEntry>
    {
        public void Configure(EntityTypeBuilder<DoubleEntry> doubleEntry)
        {
            doubleEntry.HasKey(e => e.Id);
            doubleEntry.HasIndex(e => e.Debit);
            doubleEntry.HasIndex(e => e.Credit);

            doubleEntry.HasOne(e => e.CreditAccount)
                .WithMany()
                .HasForeignKey(e => e.Credit)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            doubleEntry.HasOne(e => e.DebitAccount)
                .WithMany()
                .HasForeignKey(e => e.Debit)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            doubleEntry.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            doubleEntry.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            doubleEntry.Property(e => e.Amount).IsRequired();
        }
    }
}
