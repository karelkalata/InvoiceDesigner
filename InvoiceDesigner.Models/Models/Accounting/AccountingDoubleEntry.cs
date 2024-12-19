using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.Models.Accounting
{
	public class AccountingDoubleEntry
	{
		public int Id { get; init; }

		public EAccountingDocuments DocumentType { get; set; }

		public int DocumentId { get; set; }

		public DateTime DateTime { get; init; }

		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public int Credit { get; set; }
		public AccountingChartOfAccounts CreditAccount { get; set; } = null!;

		public int CreditAsset1 { get; set; }
		public int CreditAsset2 { get; set; }
		public int CreditAsset3 { get; set; }

		public int Debit { get; set; }
		public AccountingChartOfAccounts DebitAccount { get; set; } = null!;

		public int DebitAsset1 { get; set; }
		public int DebitAsset2 { get; set; }
		public int DebitAsset3 { get; set; }

		public decimal Count { get; set; }

		public int CurrencyId { get; set; }
		public Currency Currency { get; set; } = null!;

		public decimal Amount { get; set; }

	}
}
