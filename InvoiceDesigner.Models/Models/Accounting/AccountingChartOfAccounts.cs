using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.Models.Accounting
{
	public class AccountingChartOfAccounts
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public EAssetsTypes Asset1 { get; set; }

		public EAssetsTypes Asset2 { get; set; }

		public EAssetsTypes Asset3 { get; set; }
	}
}
