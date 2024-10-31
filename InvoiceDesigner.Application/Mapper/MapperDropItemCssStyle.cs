using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.FormDesigners;
using InvoiceDesigner.Domain.Shared.Models.FormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperDropItemCssStyle : Profile
	{
		public MapperDropItemCssStyle()
		{

			CreateMap<DropItemCssStyle, DropItemStyleEditDto>();

		}
	}
}
