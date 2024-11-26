using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICustomerService
	{
		Task<ResponsePaged<CustomerViewDto>> GetPagedCustomersAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateCustomerAsync(CustomerEditDto newEntity);

		Task<Customer> GetCustomerByIdAsync(int id);

		Task<CustomerEditDto> GetCustomerEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateCustomerAsync(CustomerEditDto editedEntity);

		Task<ResponseBoolean> DeleteCustomerAsync(int id);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int id, int modeDelete);

		Task<int> GetCustomerCountAsync();

		Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string f);

	}
}
