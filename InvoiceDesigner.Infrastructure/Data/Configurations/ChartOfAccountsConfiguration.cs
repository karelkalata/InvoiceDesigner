using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class ChartOfAccountsConfiguration : IEntityTypeConfiguration<ChartOfAccounts>
    {
        public void Configure(EntityTypeBuilder<ChartOfAccounts> chartOfAccounts)
        {
            chartOfAccounts.HasKey(e => e.Id);

            chartOfAccounts.Property(e => e.Code).IsRequired();
            chartOfAccounts.Property(e => e.Name).IsRequired();
        }
    }
}
