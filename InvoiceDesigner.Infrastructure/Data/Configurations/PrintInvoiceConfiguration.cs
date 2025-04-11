using InvoiceDesigner.Domain.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class PrintInvoiceConfiguration : IEntityTypeConfiguration<PrintInvoice>
    {
        public void Configure(EntityTypeBuilder<PrintInvoice> printInvoice)
        {
            printInvoice.HasKey(e => e.Giud);
        }
    }
}
