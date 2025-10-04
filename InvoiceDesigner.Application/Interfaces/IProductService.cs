using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Product;
using InvoiceDesigner.Application.Interfaces.Abstract;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IProductService : IABaseService<Product>
	{
		Task<ResponsePaged<ProductsViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand);


		Task<ResponseRedirect> CreateAsync(int userId, ProductEditDto productEditDto);
		Task<Product> GetByIdAsync(int id);
		Task<ProductEditDto> GetEditDtoByIdAsync(int id);
		Task<ResponseRedirect> UpdateAsync(int userId, ProductEditDto productEditDto);

		Task<int> GetCountAsync();
		Task<IReadOnlyCollection<ProductAutocompleteDto>> FilteringData(string f);
	}
}
