using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Application.Helpers.Accounting
{
	public static class HelperReports
	{
		public static decimal CalculateAccountBalance(ETypeChartOfAccount accountType, decimal debit, decimal credit)
		{
			return accountType switch
			{
				ETypeChartOfAccount.Active => debit - credit,
				ETypeChartOfAccount.Passive => credit - debit,
				ETypeChartOfAccount.Revenue => credit,
				ETypeChartOfAccount.Expense => debit,
				ETypeChartOfAccount.Equity => credit - debit,
				_ => 0
			};
		}
	}
}
