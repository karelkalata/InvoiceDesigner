using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IProductRepository
	{
		Task<IReadOnlyCollection<Product>> GetProductsAsync(int pageSize,
															int pageNumber,
															string searchString,
															Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy);

		Task<int> CreateProductAsync(Product entity);

		Task<Product?> GetProductByIdAsync(int id);

		Task<int> UpdateProductAsync(Product entity);

		Task<bool> DeleteProductAsync(Product entity);

		Task<int> GetCountProductsAsync();

		Task<bool> IsCurrencyUsedInProduct(int currencyId);
	}
}
