using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public static class CssStyleMapper
	{
		public static CssStyleEditDto ToEditDto(CssStyle style)
		{
			return new CssStyleEditDto
			{
				Id = style.Id,
				Name = style.Name,
				Value = style.Value,
				DropItemId = style.DropItemId
			};
		}
	}
}
