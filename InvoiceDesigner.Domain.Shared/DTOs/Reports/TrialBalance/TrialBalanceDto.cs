using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.DTOs.Reports.TrialBalance
{
	public class TrialBalanceDto
	{
		public int AccountCode { get; set; }
		public string AccountName { get; set; } = string.Empty;
		public ETypeChartOfAccount AccountType { get; set; }
		public decimal Amount { get; set; }
		public bool IsDebit { get; set; }
		public int CurrencyId { get; set; }
	}
}
