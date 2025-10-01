using AutoMapper;
using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperBank : Profile
	{
		public MapperBank()
		{
			CreateMap<Bank, BankAutocompleteDto>();

			CreateMap<Bank, BankEditDto>();

			CreateMap<Bank, BankPrintDto>();

		}
	}
}
