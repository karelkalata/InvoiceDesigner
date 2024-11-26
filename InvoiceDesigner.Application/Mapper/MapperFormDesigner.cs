using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperFormDesigner : Profile
	{
		public MapperFormDesigner()
		{
			CreateMap<FormDesigner, FormDesignerEditDto>();

			CreateMap<FormDesigner, FormDesignersAutocompleteDto>();

		}
	}
}
