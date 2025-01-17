using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.DTOs.Reports
{
	public class ReportTrialBalance
	{
		public string Name { get; set; } = string.Empty;
		public decimal Balance { get; set; }
		public ETypeChartOfAccount TypeChartOfAccount { get; set; }
		public string CurrencyName { get; set; } = string.Empty;
	}
}
