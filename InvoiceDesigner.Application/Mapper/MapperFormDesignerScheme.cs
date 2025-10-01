using AutoMapper;
using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperFormDesignerScheme : Profile
	{
		public MapperFormDesignerScheme()
		{
			CreateMap<FormDesignerScheme, FormDesignerSchemeEditDto>();

		}
	}
}
