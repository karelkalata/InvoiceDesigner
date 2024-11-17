namespace InvoiceDesigner.Domain.Shared.Models
{
	public class PrintInvoice
	{
		public Guid Giud { get; init; }

		public DateTime CreatedAt { get; init; } = DateTime.Now;

		public int InvoiceId { get; set; }

		public int PrintFormId { get; set; }
	}
}
