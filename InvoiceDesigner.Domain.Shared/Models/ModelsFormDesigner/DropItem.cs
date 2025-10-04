namespace InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner
{
	public class DropItem
	{
		public int Id { get; init; }
		public string UniqueId { get; set; } = string.Empty;
		public string Value { get; set; } = null!;
		public string Selector { get; set; } = null!;
		public string StartSelector { get; set; } = null!;
		public int FormDesignerSchemeId { get; set; }
		public List<CssStyle> CssStyle { get; set; } = new List<CssStyle>();
	}
}
