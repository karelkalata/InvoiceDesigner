using InvoiceDesigner.Domain.Shared.Models.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class BankReceiptConfiguration : IEntityTypeConfiguration<BankReceipt>
    {
        public void Configure(EntityTypeBuilder<BankReceipt> bankReceipt)
        {
            bankReceipt.HasKey(e => e.Id);

            bankReceipt.HasOne(e => e.Invoice)
                .WithOne()
                .HasForeignKey<BankReceipt>(e => e.InvoiceId)
                .IsRequired();


            bankReceipt.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .IsRequired();

            bankReceipt.HasOne(e => e.Customer)
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .IsRequired();

            bankReceipt.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired();

            bankReceipt.HasOne(e => e.Bank)
                .WithMany()
                .HasForeignKey(e => e.BankId)
                .IsRequired();

            bankReceipt.Property(e => e.Amount)
                .HasDefaultValue(decimal.Zero);
        }
    }
}
