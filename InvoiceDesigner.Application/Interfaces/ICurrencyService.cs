using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICurrencyService
	{
		Task<ResponsePaged<CurrencyViewDto>> GetPagedCurrenciesAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateCurrencyAsync(CurrencyEditDto currencyEditDto);

		Task<CurrencyEditDto> GetCurrencyEditDtoByIdAsync(int id);

		Task<Currency> GetCurrencyByIdAsync(int id);

		Task<ResponseRedirect> UpdateCurrencyAsync(CurrencyEditDto currencyEditDto);

		Task<ResponseBoolean> DeleteCurrencyAsync(int id);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int id, int modeDelete);

		Task<int> GetCountCurrenciesAsync();

		Task<IReadOnlyCollection<CurrencyAutocompleteDto>> GetCurrencyAutocompleteDto();

		Task<IReadOnlyCollection<CurrencyAutocompleteDto>> FilteringData(string f);

	}
}
