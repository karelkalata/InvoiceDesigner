using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Models;

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
