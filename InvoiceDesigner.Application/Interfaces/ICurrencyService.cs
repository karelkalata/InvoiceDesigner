using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICurrencyService
	{
		Task<PagedResult<CurrencyViewDto>> GetPagedCurrenciesAsync(int pageSize, int page, string searchString, string sortLabel);

		Task<ResponseRedirect> CreateCurrencyAsync(CurrencyEditDto currencyEditDto);

		Task<CurrencyEditDto> GetCurrencyEditDtoByIdAsync(int id);

		Task<Currency> GetCurrencyByIdAsync(int id);

		Task<ResponseRedirect> UpdateCurrencyAsync(CurrencyEditDto currencyEditDto);

		Task<bool> DeleteCurrencyAsync(int id);

		Task<int> GetCountCurrenciesAsync();

		Task<IReadOnlyCollection<CurrencyAutocompleteDto>> GetCurrencyAutocompleteDto();

		Task<IReadOnlyCollection<CurrencyAutocompleteDto>> FilteringData(string f);

	}
}
