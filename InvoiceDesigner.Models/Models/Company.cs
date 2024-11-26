namespace InvoiceDesigner.Domain.Shared.Models
{
	public class Company
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public string TaxId { get; set; } = string.Empty;

		public string? VatId { get; set; }

		public string? WWW { get; set; }

		public string? Email { get; set; }

		public string? Phone { get; set; }

		public string? Address { get; set; }

		public string? Info { get; set; }

		public int PaymentTerms { get; set; } = 14;

		public decimal DefaultVat { get; set; } = 21;

		public int CurrencyId { get; set; }

		public Currency Currency { get; set; } = null!;

		public ICollection<Bank> Banks { get; set; } = new List<Bank>();

	}
}
