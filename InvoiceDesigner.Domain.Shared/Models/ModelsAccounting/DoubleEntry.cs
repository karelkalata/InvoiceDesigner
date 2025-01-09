using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Models.ModelsAccounting
{
	public class DoubleEntry
	{
		public int Id { get; init; }

		public EAccountingDocument AccountingDocument { get; set; }

		public int DocumentId { get; set; }

		public DateTime DateTime { get; init; }

		public int Debit { get; set; }
		public ChartOfAccounts DebitAccount { get; set; } = null!;

		public int DebitAsset1 { get; set; }
		public int DebitAsset2 { get; set; }
		public int DebitAsset3 { get; set; }

		public int Credit { get; set; }
		public ChartOfAccounts CreditAccount { get; set; } = null!;

		public int CreditAsset1 { get; set; }
		public int CreditAsset2 { get; set; }
		public int CreditAsset3 { get; set; }

		public decimal Count { get; set; }

		public int CompanyId { get; set; }
		public Company Company { get; set; } = null!;

		public int CurrencyId { get; set; }
		public Currency Currency { get; set; } = null!;

		public decimal Amount { get; set; }

	}
}
