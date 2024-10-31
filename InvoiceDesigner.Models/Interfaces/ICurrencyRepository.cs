using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICurrencyRepository
	{
		Task<IReadOnlyCollection<Currency>> GetAllCurrenciesAsync();

		Task<IReadOnlyCollection<Currency>> GetCurrenciesAsync(int pageSize,
																int pageNumber,
																string searchString,
																Func<IQueryable<Currency>, IOrderedQueryable<Currency>> orderBy);
		Task<int> CreateCurrencyAsync(Currency entity);

		Task<Currency?> GetCurrencyByIdAsync(int id);

		Task<int> UpdateCurrencyAsync(Currency entity);

		Task<bool> DeleteCurrencyAsync(Currency entity);

		Task<int> GetCountCurrenciesAsync();
	}
}
