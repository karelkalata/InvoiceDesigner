using InvoiceDesigner.Domain.Shared.Models.Directories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> company)
        {
            company.HasKey(e => e.Id);

            company.HasIndex(e => e.Name).IsUnique();

            company.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300);

            company.Property(e => e.TaxId)
                .IsRequired()
                .HasMaxLength(50);

            company.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired();
        }
    }
}
