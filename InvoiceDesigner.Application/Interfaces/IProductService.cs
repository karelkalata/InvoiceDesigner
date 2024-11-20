using InvoiceDesigner.Domain.Shared.DTOs.Product;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IProductService
	{
		Task<ResponsePaged<ProductsViewDto>> GetPagedProductsAsync(	int pageSize, 
																	int page, 
																	string searchString, 
																	string sortLabel,
																	bool showDeleted);

		Task<ResponseRedirect> CreateProductAsync(ProductEditDto productEditDto);

		Task<Product> GetProductByIdAsync(int id);

		Task<ProductEditDto> GetProductEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateProductAsync(ProductEditDto productEditDto);

		Task<ResponseBoolean> DeleteProductAsync(int id);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int id, int modeDelete);

		Task<int> GetCountProductsAsync();

		Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string f);

	}
}
