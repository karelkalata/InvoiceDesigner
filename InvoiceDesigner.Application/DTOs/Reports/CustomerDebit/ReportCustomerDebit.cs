namespace InvoiceDesigner.Application.DTOs.Reports.CustomerDebit
{
	public class ReportCustomerDebit
	{
		public string CustomerName { get; set; } = string.Empty;
		public decimal Amount { get; set; }
		public string CurrencyName { get; set; } = string.Empty;
	}
}

