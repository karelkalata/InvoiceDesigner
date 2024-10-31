using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.FormDesigners
{
	public class DropItemEditDto
	{
		public int Id { get; set; }

		[Required]
		public string UniqueId { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		[Required]
		public string Selector { get; set; } = string.Empty;

		[Required]
		public string StartSelector { get; set; } = string.Empty;

		[Required]
		public int FormDesignerId { get; set; }

		public ICollection<DropItemStyleEditDto> CssStyleEditDto { get; set; } = new List<DropItemStyleEditDto>();
	}
}
