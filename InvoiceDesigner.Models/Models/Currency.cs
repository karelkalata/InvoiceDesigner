namespace InvoiceDesigner.Domain.Shared.Models
{
	public class Currency
	{
		public int Id { get; init; }

		public string Name { get; set; } = null!;

		public bool IsDeleted { get; set; }

		public string Description { get; set; } = string.Empty;


	}
}
