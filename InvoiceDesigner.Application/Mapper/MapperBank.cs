using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Models;

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
