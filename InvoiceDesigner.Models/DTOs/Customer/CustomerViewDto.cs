namespace InvoiceDesigner.Domain.Shared.DTOs.Customer
{
    public class CustomerViewDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string TaxId { get; set; } = string.Empty;
    }
}
