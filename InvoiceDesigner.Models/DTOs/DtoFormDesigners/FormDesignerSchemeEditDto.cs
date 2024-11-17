namespace InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners
{
	public class FormDesignerSchemeEditDto
	{
		public int Id { get; set; }

		public int Row { get; set; }

		public int Column { get; set; } = 0;

		public int FormDesignerId { get; set; }
	}
}
