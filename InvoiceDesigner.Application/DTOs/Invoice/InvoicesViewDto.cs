using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Application.DTOs.Invoice
{
	public class InvoicesViewDto
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public EStatus Status { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
		public DateTime DateTime { get; set; }
		public DateTime DueTime { get; set; }
		public string CompanyName { get; set; } = string.Empty;
		public string CustomerName { get; set; } = string.Empty;
		public string CurrencyName { get; set; } = string.Empty;
		public decimal Amount { get; set; } = decimal.Zero;
	}
}
