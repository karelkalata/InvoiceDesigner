using InvoiceDesigner.Domain.Shared.DTOs.Product;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IProductService
	{
		Task<PagedResult<ProductsViewDto>> GetPagedProductsAsync(int pageSize, int page, string searchString, string sortLabel);

		Task<ResponseRedirect> CreateProductAsync(ProductEditDto productEditDto);

		Task<Product> GetProductByIdAsync(int id);

		Task<ProductEditDto> GetProductEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateProductAsync(ProductEditDto productEditDto);

		Task<bool> DeleteProductAsync(int id);

		Task<int> GetCountProductsAsync();

		Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string f);

	}
}
