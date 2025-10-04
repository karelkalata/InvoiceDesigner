using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public static class DropItemMapper
	{
		public static DropItemEditDto ToEditDto(DropItem dropItem)
		{
			return new DropItemEditDto
			{
				Id = dropItem.Id,
				UniqueId = dropItem.UniqueId,
				Value = dropItem.Value,
				Selector = dropItem.Selector,
				StartSelector = dropItem.StartSelector,
				FormDesignerSchemeId = dropItem.FormDesignerSchemeId,
				CssStyleEditDto = dropItem.CssStyle
					.Select(CssStyleMapper.ToEditDto)
					.ToList()
			};
		}
	}
}
