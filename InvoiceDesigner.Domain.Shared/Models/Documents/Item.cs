using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Models.Documents
{
	public class Item
	{
		public int Id { get; init; }

		public int InvoiceId { get; set; }

		public Invoice Invoice { get; set; } = null!;

		public int ProductId { get; set; }

		public Product Product { get; set; } = null!;

		public decimal Price { get; set; } = decimal.Zero;

		public decimal Quantity { get; set; } = decimal.Zero;

	}

}
