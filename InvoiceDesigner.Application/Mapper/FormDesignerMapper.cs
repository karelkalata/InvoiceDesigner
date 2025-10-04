using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public static class FormDesignerMapper
	{
		public static IReadOnlyCollection<FormDesignersAutocompleteDto> ToAutocompleteDto(IReadOnlyCollection<FormDesigner> designers) => designers.Select(ToAutocompleteDto).ToList();

		public static FormDesignersAutocompleteDto ToAutocompleteDto(FormDesigner designer)
		{
			return new FormDesignersAutocompleteDto
			{
				Id = designer.Id,
				Name = designer.Name,
				AccountingDocument = designer.AccountingDocument
			};
		}

		public static FormDesignerEditDto ToEditDto(FormDesigner designer)
		{
			return new FormDesignerEditDto
			{
				Id = designer.Id,
				Name = designer.Name,
				AccountingDocument = designer.AccountingDocument,
				PageSizes = designer.PageSizes,
				DynamicHeight = designer.DynamicHeight,
				PageMargin = designer.PageMargin,
				PageOrientation = designer.PageOrientation,
				Schemes = designer.Schemes.Select(FormDesignerSchemeMapper.ToEditDto).ToList(),
				DropItems = designer.DropItems.Select(DropItemMapper.ToEditDto).ToList()
			};
		}
	}
}
