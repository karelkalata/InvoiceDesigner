using InvoiceDesigner.Application.DTOs.Currency;

namespace InvoiceDesigner.Application.DTOs.Product
{
	public class ProductPriceEditDto
	{
		public int Id { get; set; }

		public int ItemId { get; set; }

		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		public decimal Price { get; set; }

	}
}
