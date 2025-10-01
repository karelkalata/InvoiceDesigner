using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Models.Documents
{
	public class InvoiceItem
	{
		public int Id { get; init; }
		public int ItemId { get; set; }
		public decimal Price { get; set; } = decimal.Zero;
		public decimal Quantity { get; set; } = decimal.Zero;
		public Product Item { get; set; } = null!;
		public int InvoiceId { get; set; }
		public Invoice Invoice { get; set; } = null!;
		public decimal GetAmountWithoutTax()
		{
			return Price * Quantity;
		}
	}
}
