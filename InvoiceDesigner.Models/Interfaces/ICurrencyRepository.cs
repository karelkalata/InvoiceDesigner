using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICurrencyRepository
	{
		Task<IReadOnlyCollection<Currency>> GetAllCurrenciesAsync();

		Task<IReadOnlyCollection<Currency>> GetCurrenciesAsync(QueryPaged queryPaged,
																Func<IQueryable<Currency>, IOrderedQueryable<Currency>> orderBy);
		Task<int> CreateCurrencyAsync(Currency entity);

		Task<Currency?> GetCurrencyByIdAsync(int id);

		Task<int> UpdateCurrencyAsync(Currency entity);

		Task<bool> DeleteCurrencyAsync(Currency entity);

		Task<int> GetCountCurrenciesAsync(bool showDeleted = false);
	}
}
