using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.Models.ModelsAccounting
{
	public class DoubleEntrySetup
	{
		public int Id { get; init; }

		public EAccountingDocument AccountingDocument { get; set; }

		public int Credit { get; set; }
		public ChartOfAccounts CreditAccount { get; set; } = null!;

		public int Debit { get; set; }
		public ChartOfAccounts DebitAccount { get; set; } = null!;

		public EEntryMode EntryMode { get; set; }

		public EAmountType AmountType { get; set; }
	}
}
