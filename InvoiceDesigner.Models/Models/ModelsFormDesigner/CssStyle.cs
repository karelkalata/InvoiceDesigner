namespace InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner
{
	public class CssStyle
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public string Value { get; set; } = string.Empty;

		public int DropItemId { get; set; }

		public DropItem DropItem { get; set; } = null!;
	}
}
