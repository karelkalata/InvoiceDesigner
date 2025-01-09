using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs
{
	public class DoubleEntrySetupEditDto
	{
		public int Id { get; set; }

		public EAccountingDocument AccountingDocument { get; set; }

		public ChartOfAccountsAutocompleteDto DebitAccount { get; set; } = null!;

		public string DebitColumnTitle { get; set; } = string.Empty;

		public ChartOfAccountsAutocompleteDto CreditAccount { get; set; } = null!;

		public string CreditColumnTitle { get; set; } = string.Empty;

		public EEntryMode EntryMode { get; set; }

		public EAmountType AmountType { get; set; }
	}
}
