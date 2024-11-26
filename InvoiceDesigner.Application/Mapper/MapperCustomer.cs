using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperCustomer : Profile
	{
		public MapperCustomer()
		{
			CreateMap<Customer, CustomerAutocompleteDto>();

			CreateMap<Customer, CustomerEditDto>();

			CreateMap<Customer, CustomerViewDto>();

			CreateMap<Customer, CustomerPrintDto>();
		}
	}
}
