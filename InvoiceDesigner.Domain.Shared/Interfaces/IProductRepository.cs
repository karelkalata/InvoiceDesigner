using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IProductRepository
	{
		Task<IReadOnlyCollection<Product>> GetEntitiesAsync(QueryPaged queryPaged, Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy);

		Task<int> CreateAsync(Product entity);

		Task<Product?> GetByIdAsync(int id);

		Task<int> UpdateAsync(Product entity);

		Task<bool> DeleteAsync(Product entity);

		Task<int> GetCountAsync(bool showDeleted = false);

		Task<bool> IsCurrencyUsedInProduct(int currencyId);

	}
}
