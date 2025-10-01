namespace InvoiceDesigner.Application.DTOs.InvoiceItem
{
	public class InvoiceItemPrintDto
	{
		public string ProductName { get; set; } = null!;

		public decimal Price { get; set; } = decimal.Zero;

		public decimal Quantity { get; set; } = 1;

		public decimal Total => Price * Quantity;
	}
}
