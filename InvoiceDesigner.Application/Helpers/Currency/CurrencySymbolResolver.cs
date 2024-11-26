using AutoMapper;
using InvoiceDesigner.Application.Helpers.Currency;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Mapper
{
	public class CurrencySymbolResolver : IValueResolver<Currency, CurrencyPrintDto, string>
	{
		public string Resolve(Currency source, CurrencyPrintDto destination, string destMember, ResolutionContext context)
		{

			return CurrencyUnicodeHEX.GetCurrencyUnicodeHEX(source.Name);

		}
	}
}
