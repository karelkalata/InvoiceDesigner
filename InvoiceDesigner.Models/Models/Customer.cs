namespace InvoiceDesigner.Domain.Shared.Models
{
	public class Customer
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public string? TaxId { get; set; }

		public string? VatId { get; set; }
	}
}
