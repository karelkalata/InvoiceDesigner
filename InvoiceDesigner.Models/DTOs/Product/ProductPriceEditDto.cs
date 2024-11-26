using InvoiceDesigner.Domain.Shared.DTOs.Currency;

namespace InvoiceDesigner.Domain.Shared.DTOs.Product
{
	public class ProductPriceEditDto
	{
		public int Id { get; set; }

		public int ProductId { get; set; }

		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		public decimal Price { get; set; }

	}
}
