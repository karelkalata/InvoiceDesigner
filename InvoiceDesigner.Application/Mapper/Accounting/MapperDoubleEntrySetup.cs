using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;

namespace InvoiceDesigner.Application.Mapper.Accounting
{
	public class MapperDoubleEntrySetup : Profile
	{
		public MapperDoubleEntrySetup()
		{
			CreateMap<DoubleEntrySetup, DoubleEntrySetupEditDto>()
				.ForMember(
					dest => dest.DebitColumnTitle,
					opt => opt.MapFrom(src => $"{src.DebitAccount.Code} - {src.DebitAccount.Name}")
				)
				.ForMember(
					dest => dest.CreditColumnTitle,
					opt => opt.MapFrom(src => $"{src.CreditAccount.Code} - {src.CreditAccount.Name}")
				);
		}
	}
}
