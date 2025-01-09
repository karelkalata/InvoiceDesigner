using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.BankReceiptDTOs;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperBankReceipt : Profile
	{
		public MapperBankReceipt()
		{
			CreateMap<BankReceipt, BankReceiptViewDto>()
				.ForMember(
					dest => dest.BankName,
					opt => opt.MapFrom(src => src.Bank.Name)
				).ForMember(
					dest => dest.CurrencyName,
					opt => opt.MapFrom(src => src.Currency.Name)
				).ForMember(
					dest => dest.CustomerName,
					opt => opt.MapFrom(src => src.Customer.Name)
				).ForMember(
					dest => dest.CompanyName,
					opt => opt.MapFrom(src => src.Company.Name)
				);
		}
	}
}
