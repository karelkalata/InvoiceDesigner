namespace InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner
{
	public class FormDesignerScheme
	{
		public int Id { get; init; }

		public int Row { get; set; }

		public int Column { get; set; } = 0;

		public int FormDesignerId { get; set; }

	}
}
