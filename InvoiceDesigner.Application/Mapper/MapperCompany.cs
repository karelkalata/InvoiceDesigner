using AutoMapper;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperCompany : Profile
	{
		public MapperCompany()
		{
			CreateMap<Company, CompanyAutocompleteDto>();

			CreateMap<Company, CompanyEditDto>();

			CreateMap<Company, CompanyViewDto>();

			CreateMap<Company, CompanyPrintDto>();
		}
	}
}
