using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public static class FormDesignerSchemeMapper
	{
		public static FormDesignerSchemeEditDto ToEditDto(FormDesignerScheme scheme)
		{
			return new FormDesignerSchemeEditDto
			{
				Id = scheme.Id,
				Row = scheme.Row,
				Column = scheme.Column,
				FormDesignerId = scheme.FormDesignerId
			};
		}
	}
}
