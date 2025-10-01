using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Models.Documents
{
	public class Invoice
	{
		public int Id { get; init; }
		public EStatus Status { get; set; }
		public int Number { get; set; }
		public DateTime DateTime { get; set; } = DateTime.UtcNow;

		public int CompanyId { get; set; }
		public Company Company { get; set; } = null!;

		public int BankId { get; set; }
		public Bank Bank { get; set; } = null!;

		public int CurrencyId { get; set; }
		public Currency Currency { get; set; } = null!;

		public int CustomerId { get; set; }
		public Customer Customer { get; set; } = null!;

		public decimal Amount { get; set; } = decimal.Zero;
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
		public DateTime DueDate { get; set; }
		public string PONumber { get; set; } = null!;
		public decimal Vat { get; set; } = decimal.Zero;
		public bool EnabledVat { get; set; } = true;
		public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

		public decimal GetAmountTax()
		{
			return InvoiceItems.Sum(item => item.Price * item.Quantity) / 100 * Vat;
		}

		public decimal GetAmountWithoutTax()
		{
			return InvoiceItems.Sum(item => item.Price * item.Quantity);
		}

		public int GetBankId()
		{
			return BankId;
		}

		public int GetCustomerId()
		{
			return CustomerId;
		}

		public decimal GetAmountWithTax()
		{
			return Amount;
		}


	}
}
