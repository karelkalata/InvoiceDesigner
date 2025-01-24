using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Models.Abstract
{
	public abstract class AAccountingDocument
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

		public abstract decimal GetAmountTax();
		public abstract decimal GetAmountWithoutTax();

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
