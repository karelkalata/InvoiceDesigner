using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.DTOs.BankReceiptDTOs
{
	public class BankReceiptViewDto
	{
		public int Id { get; set; }

		public DateTime? DateTime { get; set; }

		public int Number { get; set; }

		public int InvoiceId { get; set; }

		public EStatus Status { get; set; }

		public bool IsDeleted { get; set; }

		public bool IsArchived { get; set; }

		public string CompanyName { get; set; } = string.Empty;

		public string CustomerName { get; set; } = string.Empty;

		public string CurrencyName { get; set; } = string.Empty;

		public string BankName { get; set; } = string.Empty;

		public decimal Amount { get; set; } = decimal.Zero;
	}
}
