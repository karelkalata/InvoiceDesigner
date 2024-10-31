using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICustomerService
	{
		Task<PagedResult<CustomerViewDto>> GetPagedCustomersAsync(int pageSize, int page, string searchString, string sortLabel);

		Task<ResponseRedirect> CreateCustomerAsync(CustomerEditDto newEntity);

		Task<Customer> GetCustomerByIdAsync(int id);

		Task<CustomerEditDto> GetCustomerEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateCustomerAsync(CustomerEditDto editedEntity);

		Task<bool> DeleteCustomerAsync(int id);

		Task<int> GetCustomerCountAsync();

		Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string f);
	}
}
