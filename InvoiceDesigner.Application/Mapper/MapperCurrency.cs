using AutoMapper;
using InvoiceDesigner.Application.Helpers;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperCurrency : Profile
	{
		public MapperCurrency()
		{
			CreateMap<Currency, CurrencyAutocompleteDto>();

			CreateMap<Currency, CurrencyEditDto>();

			CreateMap<Currency, CurrencyViewDto>();

			CreateMap<Currency, CurrencyPrintDto>()
				.ForMember(
					dest => dest.CurrencySymbol,
					opt => opt.MapFrom<CurrencySymbolResolver>()
				);

		}
	}
}
