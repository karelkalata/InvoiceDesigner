using InvoiceDesigner.Application.DTOs.DtoFormDesigners;

namespace InvoiceDesigner.Application.Responses
{
	public class ResponseFormDesignerEditDto
	{
		public FormDesignerEditDto FormDesignerEditDto { get; set; } = new FormDesignerEditDto();
		public List<DropItemEditDto> DropItemEditDto { get; set; } = new List<DropItemEditDto>();
	}
}
