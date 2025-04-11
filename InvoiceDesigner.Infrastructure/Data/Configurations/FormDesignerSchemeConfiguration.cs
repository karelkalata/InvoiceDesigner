using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceDesigner.Infrastructure.Data.Configurations
{
    public class FormDesignerSchemeConfiguration : IEntityTypeConfiguration<FormDesignerScheme>
    {
        public void Configure(EntityTypeBuilder<FormDesignerScheme> formDesignerScheme)
        {
            formDesignerScheme.HasKey(e => e.Id);
        }
    }
}
