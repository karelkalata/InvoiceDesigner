namespace InvoiceDesigner.Domain.Shared.DTOs.Currency
{
	public class CurrencyViewDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;
	}
}
