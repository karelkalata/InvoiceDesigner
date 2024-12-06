using InvoiceDesigner.Domain.Shared.DTOs.Product;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IProductService
	{
		Task<ResponsePaged<ProductsViewDto>> GetPagedProductsAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateAsync(ProductEditDto productEditDto);

		Task<Product> GetByIdAsync(int id);

		Task<ProductEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(ProductEditDto productEditDto);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity);

		Task<int> GetCountAsync();

		Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string f);
	}
}
