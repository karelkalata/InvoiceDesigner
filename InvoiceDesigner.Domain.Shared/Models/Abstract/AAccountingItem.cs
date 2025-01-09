namespace InvoiceDesigner.Domain.Shared.Models.Abstract
{
	public abstract class AAccountingItem
	{
		public int Id { get; init; }
		public int ItemId { get; set; }
		public decimal Price { get; set; } = decimal.Zero;
		public decimal Quantity { get; set; } = decimal.Zero;

		public abstract decimal GetAmountWithoutTax();
	}
}
