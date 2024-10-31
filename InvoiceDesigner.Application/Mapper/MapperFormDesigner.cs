using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.FormDesigners;
using InvoiceDesigner.Domain.Shared.Models.FormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperFormDesigner : Profile
	{
		public MapperFormDesigner()
		{

			CreateMap<FormDesigner, FormDesignerEditDto>()
				.ForMember(
					dest => dest.DropItemsDto,
					opt => opt.MapFrom(src => src.DropItems)
				);

			CreateMap<FormDesigner, FormDesignersAutocompleteDto>();

		}
	}
}
