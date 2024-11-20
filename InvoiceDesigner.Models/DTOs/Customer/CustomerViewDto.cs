namespace InvoiceDesigner.Domain.Shared.DTOs.Customer
{
	public class CustomerViewDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public bool IsDeleted { get; set; }

		public string TaxId { get; set; } = string.Empty;
	}
}
