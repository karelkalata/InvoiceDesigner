using AutoMapper;
using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

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
