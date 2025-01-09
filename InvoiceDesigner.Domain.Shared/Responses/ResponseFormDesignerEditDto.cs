using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;

namespace InvoiceDesigner.Domain.Shared.Responses
{
	public class ResponseFormDesignerEditDto
	{
		public FormDesignerEditDto FormDesignerEditDto { get; set; } = new FormDesignerEditDto();

		public List<DropItemEditDto> DropItemEditDto { get; set; } = new List<DropItemEditDto>();

	}
}
