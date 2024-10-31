namespace InvoiceDesigner.Domain.Shared.Models.FormDesigner
{
	public class DropItem
	{
		public int Id { get; init; }

		public string UniqueId { get; set; } = null!;

		public string Name { get; set; } = null!;

		public string Selector { get; set; } = null!;

		public string StartSelector { get; set; } = null!;

		public int FormDesignerId { get; set; }

		public FormDesigner FormDesigner { get; set; } = null!;

		public ICollection<DropItemCssStyle> CssStyle { get; set; } = new List<DropItemCssStyle>();


	}

}
