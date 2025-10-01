using AutoMapper;
using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperCssStyle : Profile
	{
		public MapperCssStyle()
		{
			CreateMap<CssStyle, CssStyleEditDto>();
		}
	}
}
