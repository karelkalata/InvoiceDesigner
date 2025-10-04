using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public static class CurrencyMapper
	{
		public static IReadOnlyCollection<CurrencyViewDto> ToViewDto(IReadOnlyCollection<Currency> currencies) => currencies.Select(ToViewDto).ToList();
		public static IReadOnlyCollection<CurrencyAutocompleteDto> ToAutocompleteDto(IReadOnlyCollection<Currency> currencies) => currencies.Select(ToAutocompleteDto).ToList();

		public static CurrencyAutocompleteDto ToAutocompleteDto(Currency currency)
		{
			return new CurrencyAutocompleteDto
			{
				Id = currency.Id,
				Name = currency.Name
			};
		}

		public static CurrencyViewDto ToViewDto(Currency currency)
		{
			return new CurrencyViewDto
			{
				Id = currency.Id,
				Name = currency.Name,
				IsDeleted = currency.IsDeleted,
				Description = currency.Description
			};
		}

		public static CurrencyEditDto ToEditDto(Currency currency)
		{
			return new CurrencyEditDto
			{
				Id = currency.Id,
				Name = currency.Name,
				Description = currency.Description
			};
		}

		public static CurrencyPrintDto ToPrintDto(Currency currency)
		{
			return new CurrencyPrintDto
			{
				Name = currency.Name,
				Description = currency.Description,
				CurrencySymbol = string.Empty
			};
		}
	}

}
