namespace InvoiceDesigner.Domain.Shared.DTOs.Bank
{
	public class BankViewDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string CompanyName { get; set; } = string.Empty;

		public string CurrencyName { get; set; } = string.Empty;

	}
}
