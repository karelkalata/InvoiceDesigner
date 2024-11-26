using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners
{
	public class DropItemEditDto
	{
		public int Id { get; init; }

		[Required]
		public string UniqueId { get; set; } = string.Empty;

		public string Value { get; set; } = string.Empty;

		[Required]
		public string Selector { get; set; } = string.Empty;

		[Required]
		public string StartSelector { get; set; } = string.Empty;

		[Required]
		public int FormDesignerSchemeId { get; set; }

		public List<CssStyleEditDto> CssStyleEditDto { get; set; } = new List<CssStyleEditDto>();

	}
}
