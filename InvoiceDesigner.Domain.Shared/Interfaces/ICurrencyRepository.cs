using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICurrencyRepository
	{
		Task<IReadOnlyCollection<Currency>> GetAllAsync();

		Task<IReadOnlyCollection<Currency>> GetEntitiesAsync(QueryPaged queryPaged, Func<IQueryable<Currency>, IOrderedQueryable<Currency>> orderBy);

		Task<int> CreateAsync(Currency entity);

		Task<Currency?> GetByIdAsync(int id);

		Task<int> UpdateAsync(Currency entity);

		Task<bool> DeleteAsync(Currency entity);

		Task<int> GetCountAsync(bool showDeleted = false);

	}
}
