namespace InvoiceDesigner.Domain.Shared.Models
{
	public class Invoice
	{
		public int Id { get; init; }

		public int CompanyId { get; set; }

		public Company Company { get; set; } = null!;

		public int InvoiceNumber { get; set; }

		public string PONumber { get; set; } = null!;

		public decimal Vat { get; set; } = decimal.Zero;

		public bool EnabledVat { get; set; } = true;

		public DateTime DateTime { get; set; }

		public DateTime DueDate { get; set; }

		public int CustomerId { get; set; }

		public Customer Customer { get; set; } = null!;

		public int CurrencyId { get; set; }

		public Currency Currency { get; set; } = null!;

		public int BankId { get; set; }

		public Bank Bank { get; set; } = null!;

		public decimal TotalAmount { get; set; } = decimal.Zero;

		public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

	}
}
