using InvoiceDesigner.Domain.Shared.Models.Directories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> bank)
        {
				bank.HasKey(e => e.Id);

				bank.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(200);

				bank.Property(e => e.BIC)
					.HasMaxLength(11);

				bank.Property(e => e.Account)
					.IsRequired()
					.HasMaxLength(50);

				bank.HasOne(e => e.Currency)
					.WithMany()
					.HasForeignKey(e => e.CurrencyId)
					.IsRequired();

				bank.HasOne(e => e.Company)
					.WithMany(c => c.Banks)
					.HasForeignKey(e => e.CompanyId)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
        }
    }
}
