namespace InvoiceDesigner.Domain.Shared.Models.FormDesigner
{
	public class DropItemCssStyle
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public string Value { get; set; } = string.Empty;

		public int DropItemId { get; set; }

		public DropItem DropItem { get; set; } = null!;
	}
}
