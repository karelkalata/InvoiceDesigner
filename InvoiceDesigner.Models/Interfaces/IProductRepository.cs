using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IProductRepository
	{
		Task<IReadOnlyCollection<Product>> GetProductsAsync(int pageSize,
															int pageNumber,
															string searchString,
															Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy,
															bool showDeleted = false);

		Task<int> CreateProductAsync(Product entity);

		Task<Product?> GetProductByIdAsync(int id);

		Task<int> UpdateProductAsync(Product entity);

		Task<bool> DeleteProductAsync(Product entity);

		Task<int> GetCountProductsAsync(bool showDeleted = false);

		Task<bool> IsCurrencyUsedInProduct(int currencyId);
	}
}
