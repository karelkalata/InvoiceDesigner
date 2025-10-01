using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICustomerService
	{
		Task<ResponsePaged<CustomerViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged);
		Task<ResponseRedirect> CreateAsync(int userId, CustomerEditDto newEntity);
		Task<Customer> GetByIdAsync(int id);
		Task<CustomerEditDto> GetEditDtoByIdAsync(int id);
		Task<ResponseRedirect> UpdateAsync(int userId, CustomerEditDto editedEntity);
		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand);
		Task<int> GetCountAsync();
		Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string f);
	}
}
