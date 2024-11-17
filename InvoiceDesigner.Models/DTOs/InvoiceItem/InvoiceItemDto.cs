using InvoiceDesigner.Domain.Shared.DTOs.Product;

namespace InvoiceDesigner.Domain.Shared.DTOs.InvoiceItem
{
	public class InvoiceItemDto
	{
		public int Id { get; set; }

		public ProductAutocompleteDto Product { get; set; } = null!;

		public decimal Price { get; set; } = decimal.Zero;

		public decimal Quantity { get; set; } = 1;

		public decimal Total => Price * Quantity;

	}
}

