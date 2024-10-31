using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.FormDesigners;
using InvoiceDesigner.Domain.Shared.Models.FormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperDropItem : Profile
	{
		public MapperDropItem()
		{

			CreateMap<DropItem, DropItemEditDto>()
				.ForMember(
					dest => dest.CssStyleEditDto,
					opt => opt.MapFrom(src => src.CssStyle)
				);
		}
	}
}
