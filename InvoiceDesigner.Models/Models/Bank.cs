namespace InvoiceDesigner.Domain.Shared.Models
{
	public class Bank
	{
		public int Id { get; init; }

		public string Name { get; set; } = null!;

		public string BIC { get; set; } = null!;

		public string Account { get; set; } = null!;

		public string Address { get; set; } = string.Empty;

		public int CurrencyId { get; set; }

		public Currency Currency { get; set; } = null!;

		public int CompanyId { get; set; }

		public Company Company { get; set; } = null!;
	}
}
