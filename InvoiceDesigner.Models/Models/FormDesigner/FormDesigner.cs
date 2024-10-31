namespace InvoiceDesigner.Domain.Shared.Models.FormDesigner
{
	public class FormDesigner
	{
		public int Id { get; init; }

		public string Name { get; set; } = string.Empty;

		public int Rows { get; set; } = 32;

		public int Columns { get; set; } = 3;

		public ICollection<DropItem> DropItems { get; set; } = new List<DropItem>();
	}
}
