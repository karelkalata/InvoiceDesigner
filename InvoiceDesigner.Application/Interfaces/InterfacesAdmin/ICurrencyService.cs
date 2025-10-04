using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Application.Interfaces.Abstract;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Interfaces.Admin
{
	public interface ICurrencyService : IABaseService<Currency>
	{
		Task<ResponsePaged<CurrencyViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand);

		Task<ResponseRedirect> CreateAsync(int userId, CurrencyEditDto currencyEditDto);
		Task<CurrencyEditDto> GetEditDtoByIdAsync(int id);
		Task<Currency> GetByIdAsync(int id);
		Task<string> GetNameByIdAsync(int id);
		Task<ResponseRedirect> UpdateAsync(int userId, CurrencyEditDto currencyEditDto);

		Task<int> GetCountAsync();
		Task<IReadOnlyCollection<CurrencyAutocompleteDto>> GetAutocompleteDto();
		Task<IReadOnlyCollection<CurrencyAutocompleteDto>> FilteringData(string f);
	}
}
