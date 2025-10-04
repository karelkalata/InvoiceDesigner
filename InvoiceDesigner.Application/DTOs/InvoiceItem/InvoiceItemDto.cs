using InvoiceDesigner.Application.DTOs.Product;

namespace InvoiceDesigner.Application.DTOs.InvoiceItem
{
	public class InvoiceItemDto
	{
		public int Id { get; set; }
		public ProductAutocompleteDto Item { get; set; } = null!;
		public decimal Price { get; set; } = decimal.Zero;
		public decimal Quantity { get; set; } = 1;
		public decimal Total => Price * Quantity;
	}
}

