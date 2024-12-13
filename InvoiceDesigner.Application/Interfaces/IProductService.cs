using InvoiceDesigner.Domain.Shared.DTOs.Product;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IProductService
	{
		Task<ResponsePaged<ProductsViewDto>> GetPagedAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateAsync(int userId, ProductEditDto productEditDto);

		Task<Product> GetByIdAsync(int id);

		Task<ProductEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(int userId, ProductEditDto productEditDto);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity);

		Task<int> GetCountAsync();

		Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string f);
	}
}
