using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IProductRepository
	{
		Task<IReadOnlyCollection<Product>> GetProductsAsync(QueryPaged queryPaged, 
															Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy);

		Task<int> CreateProductAsync(Product entity);

		Task<Product?> GetProductByIdAsync(int id);

		Task<int> UpdateProductAsync(Product entity);

		Task<bool> DeleteProductAsync(Product entity);

		Task<int> GetCountProductsAsync(bool showDeleted = false);

		Task<bool> IsCurrencyUsedInProduct(int currencyId);
	}
}
