using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.FormDesigners
{
	public class FormDesignerEditDto
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;

		public int Rows { get; set; } = 32;

		public int Columns { get; set; } = 3;

		public ICollection<DropItemEditDto> DropItemsDto { get; set; } = new List<DropItemEditDto>();
	}
}
