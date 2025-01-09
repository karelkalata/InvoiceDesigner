namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class Customer
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public bool IsDeleted { get; set; }

		public string? TaxId { get; set; }

		public string? VatId { get; set; }

	}
}
