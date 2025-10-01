namespace InvoiceDesigner.Application.DTOs.Reports.CustomerDebit
{
	public class CustomerDebitDto
	{
		public decimal Amount { get; set; }
		public bool IsDebit { get; set; }
		public int CurrencyId { get; set; }
		public int CustomerId { get; set; }
	}
}
