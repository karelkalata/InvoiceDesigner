using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.DTOs.Invoice
{
	public class InvoicesViewDto
	{
		public int Id { get; set; }

		public int InvoiceNumber { get; set; }

		public EInvoiceStatus Status { get; set; }

		public bool IsDeleted { get; set; }

		public bool IsArchived { get; set; }

		public DateTime DateTime { get; set; }

		public DateTime DueTime { get; set; }

		public string CompanyName { get; set; } = string.Empty;

		public string CustomerName { get; set; } = string.Empty;

		public string CurrencyName { get; set; } = string.Empty;

		public decimal TotalAmount { get; set; } = decimal.Zero;

	}
}
