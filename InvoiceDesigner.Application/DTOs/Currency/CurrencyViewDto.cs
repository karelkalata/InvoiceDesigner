namespace InvoiceDesigner.Application.DTOs.Currency
{
	public class CurrencyViewDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public bool IsDeleted { get; set; }

		public string Description { get; set; } = string.Empty;

	}
}
