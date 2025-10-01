using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Product;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IProductService
	{
		Task<ResponsePaged<ProductsViewDto>> GetPagedAsync(QueryPaged queryPaged);
		Task<ResponseRedirect> CreateAsync(int userId, ProductEditDto productEditDto);
		Task<Product> GetByIdAsync(int id);
		Task<ProductEditDto> GetEditDtoByIdAsync(int id);
		Task<ResponseRedirect> UpdateAsync(int userId, ProductEditDto productEditDto);
		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand);
		Task<int> GetCountAsync();
		Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string f);
	}
}
