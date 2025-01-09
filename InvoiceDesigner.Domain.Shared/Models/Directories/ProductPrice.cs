namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class ProductPrice
	{
		public int Id { get; init; }

		public int ProductId { get; set; }

		public int CurrencyId { get; set; }

		public Currency Currency { get; set; } = null!;

		public decimal Price { get; set; }

	}
}
