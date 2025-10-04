using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Application.Interfaces.Abstract;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICustomerService : IABaseService<Customer>
	{
		Task<ResponsePaged<CustomerViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand);


		Task<ResponseRedirect> CreateAsync(int userId, CustomerEditDto newEntity);
		Task<Customer> GetByIdAsync(int id);
		Task<CustomerEditDto> GetEditDtoByIdAsync(int id);
		Task<ResponseRedirect> UpdateAsync(int userId, CustomerEditDto editedEntity);

		Task<int> GetCountAsync();
		Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string f);
	}
}
